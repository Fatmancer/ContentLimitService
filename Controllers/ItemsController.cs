using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ContentLimitService.Models;

namespace ContentLimitService.Controllers
{
    public class ItemsController : ApiController
    {
        private ContentLimitServiceContext db = new ContentLimitServiceContext();

        // GET: api/Items
        public IQueryable<ItemDTO> GetItems()
        {
            var items = from i in db.Items select new ItemDTO()
            {
                Id = i.Id,
                Name = i.Name,
                Value = i.Value
            };
            return items;
        }

        // GET: api/Items/5
        [ResponseType(typeof(ItemDTO))]
        public async Task<IHttpActionResult> GetItem(int id)
        {
            var item = await db.Items.Select(i => new ItemDTO()
            {
                Id = i.Id,
                Name = i.Name,
                Value = i.Value
            }).SingleOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Items
        [ResponseType(typeof(ItemDTO))]
        public async Task<IHttpActionResult> PostItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Add a reference to the new item in the appropriate category's collection.
            Category category = db.Categories.First(c => c.Name == item.Name); //reference to category
            if (category == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("No Category in DB with Name = {0}", item.Name)),
                    ReasonPhrase = "Category not in DB"
                };
                throw new HttpResponseException(resp);
            }
            
            db.Items.Add(item);

            
            Item itemRef = db.Items.Find(item.Id); // reference to newly created item in the database
            category.Items.Add(itemRef);
            db.Entry(category).State = EntityState.Modified;

            await db.SaveChangesAsync();

            var dto = new ItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Value = item.Value
            };
            
            return CreatedAtRoute("DefaultApi", new { id = item.Id }, dto);
        }

        // DELETE: api/Items/5
        [ResponseType(typeof(Item))]
        public async Task<IHttpActionResult> DeleteItem(int id)
        {
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            //Remove the reference to the new item in the appropriate category's collection. Assumes the item has to be logged in item due to the workflow in POST.
            Category category = db.Categories.First(c => c.Name == item.Name); //reference to category
            category.Items.Remove(item);
            db.Entry(category).State = EntityState.Modified;

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            return Ok(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.Id == id) > 0;
        }
    }
}