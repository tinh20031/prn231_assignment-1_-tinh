using BusinessObject;
using DataAccess;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eStore.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly ProductRepository _productRepository;
		public ProductController(ProductRepository productRepository)
		{
			_productRepository = productRepository;
		}
		[HttpGet]
		public IActionResult GetAllProduct () { 
		var products = _productRepository.GetAllProducts ();
			return Ok(products);
		}

		// GET: api/product/1
		[HttpGet("{id}")]
		public IActionResult GetProductById(int id)
		{
			var product = _productRepository.FindProductById(id);
			if (product == null)
			{
				return NotFound();
			}
			return Ok(product);
		}

		// POST: api/product (Thêm mới sản phẩm)
		[HttpPost]
		public IActionResult CreateProduct([FromBody] Products product)
		{
			if (product == null)
			{
				return BadRequest();
			}
			_productRepository.SaveProduct(product);
			return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
		}

		// PUT: api/product/1 (Cập nhật sản phẩm)
		[HttpPut("{id}")]
		public IActionResult UpdateProduct(int id, [FromBody] Products product)
		{
			if (product == null || product.ProductId != id)
			{
				return BadRequest();
			}

			var existingProduct = _productRepository.FindProductById(id);
			if (existingProduct == null)
			{
				return NotFound();
			}

			_productRepository.UpdateProduct(product);
			return NoContent();
		}

		// DELETE: api/product/1 (Xóa sản phẩm)
		[HttpDelete("{id}")]
		public IActionResult DeleteProduct(int id)
		{
			var product = _productRepository.FindProductById(id);
			if (product == null)
			{
				return NotFound();
			}

			_productRepository.DeleteProduct(product);
			return NoContent();
		}
	}
}
