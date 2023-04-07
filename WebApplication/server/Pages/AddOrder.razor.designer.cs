using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using WebApplication.Models.LocalDb;
using Microsoft.EntityFrameworkCore;
using WebApplication.Services;
using BeautySaloon.Api.Dto.Responses.Person;
using BeautySaloon.Api.Services;
using WebApplication.Wrappers;
using AutoMapper;
using BeautySaloon.Api.Dto.Responses.Subscription;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.Api.Dto.Requests.Person;
using BeautySaloon.Api.Dto.Requests.Subscription;
using BeautySaloon.Api.Dto.Requests.Order;

namespace WebApplication.Pages
{
    public partial class AddOrderComponent : ComponentBase
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
        protected ISubscriptionHttpClient SubscriptionHttpClient { get; set; }

        [Inject]
        protected IOrderHttpClient OrderHttpClient { get; set; }

        [Inject]
        protected IHttpClientWrapper HttpClientWrapper { get; set; }

        [Inject]
        protected IMapper Mapper { get; set; }

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

        private IReadOnlyCollection<GetSubscriptionListItemResponseDto> _getSubscriptionsResult;

        protected IReadOnlyCollection<GetSubscriptionListItemResponseDto> GetSubscriptionsResult
        {
            get
            {
                return _getSubscriptionsResult;
            }
            set
            {
                if (!object.Equals(_getSubscriptionsResult, value))
                {
                    _getSubscriptionsResult = value;
                    Reload();
                }
            }
        }

        private OrderRequest _order;

        protected OrderRequest Order
        {
            get
            {
                return _order;
            }
            set
            {
                if (!object.Equals(_order, value))
                {
                    _order = value;
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
            var personsTask = GetPersonsAsync();
            var subscriptionsTask = GetSubscriptionsAsync();

            await Task.WhenAll(personsTask, subscriptionsTask);

            GetPersonsResult = personsTask.Result;
            GetSubscriptionsResult = subscriptionsTask.Result;

            Order = new OrderRequest();
        }

        protected async Task Form0Submit(OrderRequest args)
        {
            var request = Mapper.Map<CreateOrderRequestDto>(Order);

            var isSuccess = await HttpClientWrapper.SendAsync((accessToken) => OrderHttpClient.CreateAsync(accessToken, request, CancellationToken.None));

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

            async Task<PageResponseDto<GetPersonListItemResponseDto>?> GetListAsync(int number, int size)
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

        private async Task<IReadOnlyCollection<GetSubscriptionListItemResponseDto>> GetSubscriptionsAsync()
        {
            var pageSize = 100;
            var pageNumber = 1;
            var totalCount = 1;

            async Task<PageResponseDto<GetSubscriptionListItemResponseDto>?> GetListAsync(int number, int size)
            {
                var subscriptions = await HttpClientWrapper.SendAsync((accessToken)
                    => SubscriptionHttpClient.GetListAsync(accessToken, new GetSubscriptionListRequestDto { Page = new PageRequestDto(number, size) }, CancellationToken.None));

                if (subscriptions == default)
                {
                    return null;
                }

                totalCount = subscriptions.TotalCount - pageSize;
                pageNumber++;

                return subscriptions;
            }

            var result = new List<GetSubscriptionListItemResponseDto>();

            while (totalCount > 0)
            {
                var subscriptions = await GetListAsync(pageNumber, pageSize);

                if (subscriptions != null)
                {
                    result.AddRange(subscriptions.Items);
                }
            }

            return result.ToArray();
        }

        public record OrderRequest
        {
            public Guid PersonId { get; set; }

            public string? Comment { get; set; }

            public List<Guid> SubscriptionIds { get; set; }
        }
    }
}
