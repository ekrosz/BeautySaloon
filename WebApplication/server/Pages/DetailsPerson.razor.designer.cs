using AutoMapper;
using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.Person;
using BeautySaloon.Api.Services;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Handlers;
using WebApplication.Wrappers;

namespace WebApplication.Pages;

public partial class DetailsPersonComponent : ComponentBase
{
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

    public void Reload()
    {
        InvokeAsync(StateHasChanged);
    }

    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected IPersonHttpClient PersonHttpClient { get; set; }

    [Inject]
    protected IHttpClientWrapper HttpClientWrapper { get; set; }

    [Inject]
    protected IMapper Mapper { get; set; }

    [Parameter]
    public dynamic Id { get; set; }

    private PersonRequest _person;

    protected PersonRequest Person
    {
        get
        {
            return _person;
        }
        set
        {
            if (!object.Equals(_person, value))
            {
                _person = value;
                Reload();
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await Load();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected async Task Load()
    {
        var person = await HttpClientWrapper.SendAsync<GetPersonResponseDto>((accessToken) => PersonHttpClient.GetAsync(accessToken, Guid.Parse(Id), CancellationToken.None));

        if (person == default)
        {
            return;
        }

        Person = Mapper.Map<PersonRequest>(person);
    }

    protected async Task Button2Click(MouseEventArgs args)
    {
        NavigationManager.NavigateTo("/persons");
    }

    public record PersonRequest
    {
        public Guid Id { get; set; }

        public FullName Name { get; set; } = FullName.Empty;

        public DateTime BirthDate { get; set; }

        public string? Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public IReadOnlyCollection<SubscriptionRequest> Subscriptions { get; init; } = Array.Empty<SubscriptionRequest>();

        public record SubscriptionRequest
        {
            public Guid Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public int? LifeTimeInDays { get; set; }

            public decimal Price { get; set; }

            public PersonSubscriptionStatus Status { get; set; }

            public DateTime PaidOn { get; set; }
        }
    }
}
