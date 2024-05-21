using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService couponService;

        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? couponsList = new();
            ResponseDTO responseDto  = await couponService.GetAllCouponsAsync();
            if (responseDto != null & responseDto.IsSuccess) 
            {
                couponsList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
            return View(couponsList);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? responseDto = await couponService.CreateCouponAsync(couponDto);
                if (responseDto != null & responseDto.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.Message;
                }
            }
            return View(couponDto);
        }

        
        
        public async Task<IActionResult> CouponDelete(int couponId)
        {
            if (ModelState.IsValid)
            {
				ResponseDTO? response = await couponService.GetCouponsByIdAsync(couponId);
				//ResponseDTO? response = await couponService.DeleteCouponAsync(couponId);
				if (response != null && response.IsSuccess)
                {
                    CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
					return View(model);
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return NotFound();

        }
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
                
            ResponseDTO? response = await couponService.DeleteCouponAsync(couponDto.CouponId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            //}
            return View(couponDto);

        }
    }
}
