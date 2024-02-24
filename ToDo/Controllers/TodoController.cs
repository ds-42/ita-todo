using Microsoft.AspNetCore.Mvc;
using ToDo.Apis;
using ToDo.Models;

namespace ToDo.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodoController : ControllerBase
    {
        public TodoController()
        {
        }


        [HttpGet]
        public IActionResult Get(int limit = 10, int offset = 0)
        {
            return Ok(TodoApi.GetItems(limit, offset));
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var item = TodoApi.Find(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        protected IActionResult GetIsDoneState(Todo item) 
        {
            return Ok(new
            {
                item.Id,
                item.IsDone,
            });
        }


        [HttpGet("{id}/IsDone")]
        public IActionResult GetIsDone(int id)
        {
            var item = TodoApi.Find(id);

            if (item == null)
                return NotFound();

            return GetIsDoneState(item);
        }


        [HttpPost]
        public IActionResult AddItem(string label)
        {
            var item = TodoApi.Add(label);

            return Created($"todos/{item.Id}", item);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, string label)
        {
            var item = TodoApi.Update(id, label);

            if (item == null)
                return NotFound();

            return Ok(item);
        }


        [HttpPatch("{id}/IsDone")]
        public IActionResult DoneItem(int id)
        {
            var item = TodoApi.Done(id);

            if (item == null)
                return NotFound();

            return GetIsDoneState(item);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            var item = TodoApi.Delete(id);

            if (item == null)
                return NotFound();

            return Ok();
        }
    }
}
