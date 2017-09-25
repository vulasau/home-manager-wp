using HomeManager.Cash.Entities;
using HomeManager.Entities;

namespace HomeManager.Cash.Interfaces
{
    public interface ICashService
    {
        void ProcessOperation(Operation operation);
        void ProcessConversion(Conversion operation);
        ConversionPreview PreviewConversion(Conversion operation);
    }
}
