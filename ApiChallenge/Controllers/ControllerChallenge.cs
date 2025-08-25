using System.Text.Json;
using Application.Dto;
using Application.Interfaces;
using Infraestructure.ElascticSearch;
using Infraestructure.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiChallenge.Controllers
{
    [Route("/api/v1/permissions")]
    [ApiController]
    public class ControllerChallenge (IChallanceService challanceService,ElacticService elastic,KafkaService kafka) : Controller
    {
        private IChallanceService _challanceService { get; } = challanceService;
        private   ElacticService _elastic{ get; } = elastic;
        private   KafkaService _kafka{ get; } = kafka;
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = _challanceService.GetAll();

            await _kafka.ProduceAsync(JsonSerializer.Serialize(new
            {
                Id = Guid.NewGuid(),
                NameOperation = "get"
            }));

            return Ok(result);
        }
        [HttpPost]
        [Route("request")]
        public async Task<IActionResult> request([FromBody] PermissionsDto input)
        {
            var result = await _challanceService.Create(input);
            // Indexar en Elasticsearch
            await _elastic.IndexPermissionAsync(result);

            // Publicar en Kafka
            await _kafka.ProduceAsync(JsonSerializer.Serialize(new {
                Id = Guid.NewGuid(),
                NameOperation = "request"
            }));
            return Ok(result);
        }
        [HttpPut]
        [Route("modify")]
        public async Task<IActionResult> modify([FromBody] PermissionsDto input)
        {
            var result = await _challanceService.Update(input);
            await _kafka.ProduceAsync(JsonSerializer.Serialize(new {
                Id = Guid.NewGuid(),
                NameOperation = "modify"
            }));
            return Ok(result);
        }
    }
}
