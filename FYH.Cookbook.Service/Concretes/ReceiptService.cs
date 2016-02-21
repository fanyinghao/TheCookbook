using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.DAO.Abstracts;
using FYH.Cookbook.Model.DBEntity;
using FYH.Cookbook.Model.ViewModels;
using FYH.Cookbook.Service.Abstracts;

namespace FYH.Cookbook.Service.Concretes
{
    public class ReceiptService: IReceiptService
    {
        private IBaseRepository BaseRepository { get; set; }

        public void AddReceipt(AddReceiptViewModel viewModel)
        {
            var model = new Receipt
            {
                Name = viewModel.ReceiptName,
                CreatedDate = DateTime.Now
            };
            BaseRepository.AddOrUpdateEntity(model);
        }
    }
}
