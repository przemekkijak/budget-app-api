using BudgetApp.Domain.Entities;
using BudgetApp.Domain.Enums;
using MediatR;

namespace BudgetApp.Core.Features.Budgets.Queries
{
    public class BudgetActionAuthCommand : IRequest<bool>
    {
        public BudgetActionAuthCommand(string userId, BudgetEntity budgetEntity, TransactionActionEnum transactionActionEnum)
        {
            UserId = userId;
            BudgetEntity = budgetEntity;
            TransactionActionEnum = transactionActionEnum;
        }

        public string UserId { get; }
        public BudgetEntity BudgetEntity { get; }
        public TransactionActionEnum TransactionActionEnum { get; }
    }

    // public class BudgetActionAuthHandler : IRequestHandler<BudgetActionAuthCommand, bool>
    // {
    //     private readonly IPermissionRepository _permissionRepository;
    //
    //     public BudgetActionAuthHandler(IPermissionRepository permissionRepository)
    //     {
    //         _permissionRepository = permissionRepository;
    //     }
    //
    //     public async Task<bool> Handle(BudgetActionAuthCommand request, CancellationToken cancellationToken)
    //     {
    //         var permission = await _permissionRepository.GetPermissionAsync(request.UserId, request.BudgetEntity.Id, cancellationToken);
    //         if (permission == null)
    //         {
    //             return false;
    //         }
    //
    //         return permission.TransactionActionEnum.HasFlag(request.TransactionActionEnum);
    //     }
    // }
}