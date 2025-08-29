using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Api.Data.Access
{
    public class DataCategoria
    {
        private readonly MySQLiteContext _context;
        public DataCategoria(MySQLiteContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Categoria categoria)
        {
            bool result;

            try
            {
                _context.Categoria.Add(categoria);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la categoria: " + ex.Message);
            }

            return result;
        }

        public async Task<bool> Delete(int id)
        {
            bool result;

            try
            {
                _context.Categoria.Remove(_context.Categoria.Find(id)!);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la categoria: " + ex.Message);
            }

            return result;
        }

        public async Task<IEnumerable<Categoria>> GetAll()
        {
            IEnumerable<Categoria> categorias;

            try { 
                categorias = await _context.Categoria.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las categorias: " + ex.Message);
            }

            return categorias;
        }

        public async Task<Categoria> GetById(int id)
        {
            Categoria categoria;

            try
            {
                categoria = await _context.Categoria.FindAsync(id) ?? throw new Exception("Categoria no encontrada");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la categoria: " + ex.Message);
            }

            return categoria;
        }   

        public async Task<bool> Update(Categoria categoria)
        {
            bool result;

            try
            {
                _context.Categoria.Update(categoria);
                await _context.SaveChangesAsync();
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la categoria: " + ex.Message);
            }

            return result;
        }
    }
}
