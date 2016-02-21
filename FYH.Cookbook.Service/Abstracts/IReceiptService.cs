using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYH.Cookbook.Model.ViewModels;

namespace FYH.Cookbook.Service.Abstracts
{
    public interface IReceiptService
    {
        void AddReceipt(AddReceiptViewModel viewModel);
    }
}
