using AuctionAPI.Models;
using AuctionAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionContext _AuctionContext;

        public AuctionController(AuctionContext auctionContext)
        {
            _AuctionContext = auctionContext;
        }

        //Create and Edit
        [HttpPost]
        public JsonResult CreateEdit(AuctionInfo auctionInfo)
        {
            if (auctionInfo.UID == null)
            {
                _AuctionContext.AuctionInfos.Add(auctionInfo);
            }
            else
            {
                var auctionInDB = _AuctionContext.AuctionInfos.Find(auctionInfo.UID);

                if (auctionInDB == null)
                    return new JsonResult(NotFound());

                auctionInDB = auctionInfo;
            }
            _AuctionContext.SaveChanges();

            return new JsonResult(Ok(auctionInfo));
        }

        //Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _AuctionContext.AuctionInfos.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        //Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _AuctionContext.AuctionInfos.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            _AuctionContext.AuctionInfos.Remove(result);
            _AuctionContext.SaveChanges();

            return new JsonResult(Ok(result));
        }

        //GetAll
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _AuctionContext.AuctionInfos.ToList();

            return new JsonResult(Ok(result));
        }
    }
}