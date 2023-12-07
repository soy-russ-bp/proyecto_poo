using ARJE.SignPractice.DataModels;
using ARJE.SignPractice.ViewModels;
using ARJE.SignPractice.Views;

namespace ARJE.SignPractice.Controllers
{
    public sealed class PracticeViewController : ViewControllerBase<PracticeView, PracticeDataModel, PracticeViewModel>
    {
        public PracticeViewController(PracticeDataModel dataModel)
            : base(dataModel)
        {
        }
    }
}
