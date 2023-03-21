using AutoMapper;
using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.Person;
using BeautySaloon.Api.Services;
using BeautySaloon.DAL.Entities.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Handlers;

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
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected DialogService DialogService { get; set; }

    [Inject]
    protected TooltipService TooltipService { get; set; }

    [Inject]
    protected ContextMenuService ContextMenuService { get; set; }

    [Inject]
    protected NotificationService NotificationService { get; set; }

    [Inject]
    protected IPersonHttpClient PersonHttpClient { get; set; }

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

    protected override async Task OnInitializedAsync()
    {
        await Load();
    }
    protected async Task Load()
    {
        try
        {
            var person = await PersonHttpClient.GetAsync(Guid.Parse(Id), CancellationToken.None);

            Person = Mapper.Map<PersonRequest>(person);
        }
        catch (CustomApiException ex)
        {
            NotificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = ex.Message,
                Detail = ex.Details.ErrorMessage
            });

            if (ex.Details.StatusCode == System.Net.HttpStatusCode.Unauthorized || ex.Details.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                NavigationManager.NavigateTo("/login");
            }
        }
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
        }
    }
}
