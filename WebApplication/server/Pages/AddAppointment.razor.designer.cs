﻿using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using BeautySaloon.Api.Dto.Responses.Person;
using BeautySaloon.Api.Services;
using WebApplication.Wrappers;
using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.Api.Dto.Requests.Person;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace WebApplication.Pages
{
    public partial class AddAppointmentComponent : ComponentBase
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
        protected IPersonHttpClient PersonHttpClient { get; set; }

        [Inject]
        protected IAppointmentHttpClient AppointmentHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        [Inject]
        protected IMapper Mapper { get; set; }

        [Parameter]
        public DateTime AppointmentDate { get; set; }

        private IReadOnlyCollection<GetPersonListItemResponseDto> _getPersonsResult;

        protected IReadOnlyCollection<GetPersonListItemResponseDto> GetPersonsResult
        {
            get
            {
                return _getPersonsResult;
            }
            set
            {
                if (!object.Equals(_getPersonsResult, value))
                {
                    _getPersonsResult = value;
                    Reload();
                }
            }
        }

        private IReadOnlyCollection<PersonSubscriptionCosmeticServiceResponse> _getPersonSubscriptionsResult;

        protected IReadOnlyCollection<PersonSubscriptionCosmeticServiceResponse> GetPersonSubscriptionsResult
        {
            get
            {
                return _getPersonSubscriptionsResult;
            }
            set
            {
                if (!object.Equals(_getPersonSubscriptionsResult, value))
                {
                    _getPersonSubscriptionsResult = value;
                    Reload();
                }
            }
        }

        private AppointmentRequest _appointment;

        protected AppointmentRequest Appointment
        {
            get
            {
                return _appointment;
            }
            set
            {
                if (!object.Equals(_appointment, value))
                {
                    _appointment = value;
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
            GetPersonsResult = await GetPersonsAsync();

            Appointment = new AppointmentRequest()
            {
                AppointmentDate = AppointmentDate
            };
        }

        protected async Task OnPersonSelectedEvent(object args)
        {
            var personSubscriptions = await HttpClientWrapper.SendAsync((accessToken) => PersonHttpClient.GetSubscriptionsAsync(accessToken, (Guid)args, CancellationToken.None));

            GetPersonSubscriptionsResult = personSubscriptions.Items
                .GroupBy(x => x.Subscription.Id)
                .SelectMany(x => new[] { new PersonSubscriptionCosmeticServiceResponse
                {
                    Id = x.First().Id,
                    CosmeticServiceName = x.First().CosmeticService.Name,
                    SubscriptionName = x.First().Subscription.Name
                } }.Concat(x.GroupBy(y => y.CosmeticService.Id).Select(y => new PersonSubscriptionCosmeticServiceResponse
                {
                    Id = y.First().Id,
                    CosmeticServiceName = y.First().CosmeticService.Name,
                    CosmeticServiceCount = y.Count(),
                    SubscriptionName = null!
                }))).ToArray();
        }

        protected async Task Form0Submit(AppointmentRequest args)
        {
            var request = Mapper.Map<CreateAppointmentRequestDto>(Appointment);

            var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => AppointmentHttpClient.CreateAsync(accessToken, request, CancellationToken.None));

            DialogService.Close(isSuccess);
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(false);
        }

        private async Task<IReadOnlyCollection<GetPersonListItemResponseDto>> GetPersonsAsync()
        {
            var pageSize = 100;
            var pageNumber = 1;
            var totalCount = 1;

            async Task<PageResponseDto<GetPersonListItemResponseDto>> GetListAsync(int number, int size)
            {
                var persons = await HttpClientWrapper.SendAsync((accessToken)
                    => PersonHttpClient.GetListAsync(accessToken, new GetPersonListRequestDto { Page = new PageRequestDto(number, size) }, CancellationToken.None));

                if (persons == default)
                {
                    return null;
                }

                totalCount = persons.TotalCount - pageSize;
                pageNumber++;

                return persons;
            }

            var result = new List<GetPersonListItemResponseDto>();

            while (totalCount > 0)
            {
                var persons = await GetListAsync(pageNumber, pageSize);

                if (persons != null)
                {
                    result.AddRange(persons.Items);
                }
            }

            return result.ToArray();
        }

        public record AppointmentRequest
        {
            public Guid PersonId { get; set; }

            public DateTime AppointmentDate { get; set; } = DateTime.Now.AddHours(1);
            
            public string? Comment { get; set; }

            public List<Guid> PersonSubscriptionIds { get; set; } = new List<Guid>();
        }

        public record PersonSubscriptionCosmeticServiceResponse
        {
            public Guid Id { get; set; }

            public string? SubscriptionName { get; set; }

            public string CosmeticServiceName { get; set; }

            public int CosmeticServiceCount { get; set; }

            public bool IsGroup => SubscriptionName != null;
        }
    }
}
