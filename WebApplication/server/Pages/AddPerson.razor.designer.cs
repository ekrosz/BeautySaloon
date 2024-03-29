﻿using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using BeautySaloon.Api.Services;
using AutoMapper;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.Api.Dto.Requests.Person;
using WebApplication.Handlers;
using WebApplication.Wrappers;

namespace WebApplication.Pages
{
    public partial class AddPersonComponent : ComponentBase
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
            Person = new PersonRequest();
        }

        protected async Task Form0Submit(PersonRequest args)
        {
            var request = Mapper.Map<CreatePersonRequestDto>(Person);

            var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => PersonHttpClient.CreateAsync(accessToken, request, CancellationToken.None));

            DialogService.Close(isSuccess);
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(false);
        }

        public record PersonRequest
        {
            public FullName Name { get; set; } = FullName.Empty;

            public DateTime BirthDate { get; set; } = new DateTime(1990, 1, 1);

            public string? Email { get; set; }

            public string PhoneNumber { get; set; } = string.Empty;
        }
    }
}
