using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QRController : Controller
    {
        [HttpGet]
        public IActionResult CreateQR(string s)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(s, QRCodeGenerator.ECCLevel.Q))
                {
                    using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                    {
                        byte[] qrCodeImage = qrCode.GetGraphic(20);
                        return File(qrCodeImage, "image/png");
                    }
                }
                
            }
            
        }
    }
}
