﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiServico.Models.Dtos;
using ApiServico.Models;
using ApiServico.DataContexts;
using Microsoft.EntityFrameworkCore;
using ApiServico.Controllers;

namespace ApiServico.Controllers
{
    [Route("/chamados")]
    [ApiController]
    public class ChamadoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarTodos(
                [FromQuery] string? search,
                [FromQuery] string? situacao
            )
        {
            var query = _context.Chamados.AsQueryable();

            if(search is not null)
            {
                query = query.Where(x => x.Titulo.Contains(search));
            }

            if(situacao is not null)
            {
                query = query.Where(x => x.Status.Equals(situacao));
            }

            var chamados = await query
                .Include(p => p.Prioridade)
                .Select(c => new
                {
                    c.Id,
                    c.Titulo,
                    c.Status,
                    Prioridade = new {c.Prioridade.Nome}
                })
                .ToListAsync();

            return Ok(chamados);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var chamado = await _context.Chamados.FirstOrDefaultAsync(x => x.Id == id);

            if(chamado is null)
            {
                return NotFound();
            }

            return Ok(chamado);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] ChamadoDto novoChamado)
        {
            var prioridade = await _context.Prioridades.FirstOrDefaultAsync(x => x.Id == novoChamado.PrioridadeId);
            
            if(prioridade is null)
            {
                return NotFound("Prioridade  informada não existe");
            }

            var chamado = new Chamado() { 
                Titulo = novoChamado.Titulo, 
                Descricao = novoChamado.Descricao, 
                PrioridadeId = novoChamado.PrioridadeId 
            };

            await _context.Chamados.AddAsync(chamado);
            await _context.SaveChangesAsync();

            return Created("", chamado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] ChamadoDto atualizacaoChamado)
        {
            var chamado = await _context.Chamados.FirstOrDefaultAsync(x => x.Id == id);
            var prioridade = await _context.Prioridades.FirstOrDefaultAsync(x => x.Id == atualizacaoChamado.PrioridadeId);


            if (chamado is null)
            {
                return NotFound();
            }

            if(prioridade is null)
            {
                return NotFound("Prioridade  informada não existe");
            }


            chamado.Titulo = atualizacaoChamado.Titulo;
            chamado.Descricao = atualizacaoChamado.Descricao;
            chamado.PrioridadeId = atualizacaoChamado.PrioridadeId;

            _context.Chamados.Update(chamado);
            await _context.SaveChangesAsync();

            return Ok(chamado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            var chamado = await _context.Chamados.FirstOrDefaultAsync(x => x.Id == id);

            if (chamado is null)
            {
                return NotFound();
            }

            _context.Chamados.Remove(chamado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
