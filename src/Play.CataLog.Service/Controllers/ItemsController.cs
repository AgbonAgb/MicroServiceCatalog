using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.CataLog.Service.Dtos;
//using Play.CataLog.Service.Repository;
using Play.CataLog.Service.Entities;
using Play.Common;

namespace Play.CataLog.Service.Controllers
{
    [ApiController]//attributs
    //https://localhost:5001/items
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemRepository;
        public ItemsController(IRepository<Item> _itemRepository)
        {
            itemRepository = _itemRepository;
        }
        //private readonly ItemsRepository itemRepository = new();
        //static makes it not to be recreated any sigle time 
        //request is made from API
        // private static readonly List<ItemDto> items = new()
        // {
        //     new ItemDto(Guid.NewGuid(),"Portion","Restore small amt of Hp",5, System.DateTimeOffset.UtcNow),
        //      new ItemDto(Guid.NewGuid(),"Antidote","Cure poison",7, System.DateTimeOffset.UtcNow),
        //       new ItemDto(Guid.NewGuid(),"Bronze Sword","Repairs small amount of damage",20, System.DateTimeOffset.UtcNow)
        // };
        //https://localhost:5001/items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemRepository.GetAllAsync()).Select(itm => itm.asDto());//convert enity item to itemDto
            return items;

        }

        //https://localhost:5001/items/123
        [HttpGet("{Id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid Id)
        {
            // var itm = items.Where(itmm => itmm.Id == Id).SingleOrDefault();
            var itm = await itemRepository.GetAsync(Id);

            if (itm == null)
            {
                return NotFound();
            }
            return itm.asDto();
        }

        //Create
        //https://localhost:5001/items/CreateItem
        [HttpPost("CreateItem")]
        //we used ActionResult because we want specific return type
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreatedItemDto Crd)
        {
            //var itm = new ItemDto(Guid.NewGuid(), Crd.Name, Crd.Description, Crd.Price, System.DateTimeOffset.UtcNow);
            //items.Add(itm);

            var itm = new Item
            {
                Name = Crd.Name,
                Description = Crd.Description,
                Price = Crd.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemRepository.CreateAsync(itm);

            return CreatedAtAction(nameof(GetByIdAsync), new { id = itm.Id }, itm);
        }
        //we used IActionResult because we do not want specific return type
        //https://localhost:5001/items/UpdateItem/54f6842c-c7a3-48fe-9400-c70a3cae299d
        [HttpPut("UpdateItem/{Id}")]
        public async Task<IActionResult> UpdateItemAsync(Guid Id, UpdateItemDto Crd)
        {

            var Existingitm = await itemRepository.GetAsync(Id);
            if (Existingitm == null)
            {
                return NotFound();
            }

            Existingitm.Name = Crd.Name;
            Existingitm.Description = Crd.Description;
            Existingitm.Price = Crd.Price;

            await itemRepository.UpdateDb(Existingitm);

            //var Existingitm = items.Where(itmm => itmm.Id == Id).SingleOrDefault();
            //if (Existingitm == null)
            //{
            //   return NotFound();
            //}

            //var updatedItem = Existingitm with
            //{
            //    Name = Crd.Name,
            //    Description = Crd.Description,
            //    Price = Crd.Price
            //};

            //Find index in list
            //var index = items.FindIndex(Existingitm => Existingitm.Id == Id);
            //items[index] = updatedItem;

            return NoContent();
        }
        //delete
        //https://localhost:5001/items/DeleteItem/54f6842c-c7a3-48fe-9400-c70a3cae299d
        [HttpDelete("DeleteItem/{Id}")]
        public async Task<IActionResult> DeleteItemAsync(Guid Id)
        {
            var Existingitm = await itemRepository.GetAsync(Id);
            if (Existingitm == null)
            {
                return NotFound();
            }

            await itemRepository.RemoveAsync(Existingitm.Id);

            //var index = items.FindIndex(Existingitm => Existingitm.Id == Id);
            // if (index < 0)
            //{
            //    return NotFound();
            // }

            // items.RemoveAt(index);
            return NoContent();
        }

    }
}