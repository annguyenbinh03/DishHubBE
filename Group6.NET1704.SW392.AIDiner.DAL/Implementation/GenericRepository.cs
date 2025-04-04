﻿using System.Linq.Expressions;
using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Data;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Group6.NET1704.SW392.AIDiner.DAL.Implementation;
public class GenericRepository<T> : IGenericRepository<T> where T : class  
{  
    private DishHub5Context _context;  
    private DbSet<T> _dbSet;  
  
    public GenericRepository(DishHub5Context context)  
    {        _context = context;  
        _dbSet = context.Set<T>();  
    }  
    public async Task<PagedResult<T>> GetAllDataByExpression(Expression<Func<T, bool>>? filter,  
       int? pageNumber, int? pageSize,  
       Expression<Func<T, object>>? orderBy = null,  
       bool isAscending = true,  
       params Expression<Func<T, object>>[]? includes)  
    {        IQueryable<T> query = _dbSet;  
  
        if (filter != null)  
        {            query = query.Where(filter);  
        }        if (includes != null)  
        {            foreach (var include in includes)  
            {                query = query.Include(include);  
            }        }  
        if (orderBy != null)  
        {            query = isAscending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);  
        }  
        var totalItems = await query.CountAsync();  
        var result = new PagedResult<T>  
        {            Items = null,  
            TotalPages = 0  
        };  
        if ( (pageSize != null && pageNumber != null ) && (   pageNumber > 0 && pageSize > 0))  
        {            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);  
            query = query.Skip(((pageNumber ?? 1 ) - 1) * (pageSize ?? int.MaxValue)).Take(pageSize ?? int.MaxValue);  
            result.Items = await query.AsNoTracking().ToListAsync();  
            result.TotalPages = totalPages;  
            return result;  
        }  
        var data = await query.ToListAsync();  
        result.Items = data;  
        result.TotalPages = data.Count() > 0 ? 1 : 0;  
        return result;  
    }  
    public async Task<T> GetById(object id)  
    {        return await _dbSet.FindAsync(id);  
    }

    public async Task<T?> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        var keyName = _context.Model
                              .FindEntityType(typeof(T))?
                              .FindPrimaryKey()?
                              .Properties
                              .Select(x => x.Name)
                              .FirstOrDefault();

        if (keyName == null)
            throw new InvalidOperationException($"Can not find primary key for {typeof(T).Name}");

        return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, keyName) == id);
    }

    public async Task<T> Insert(T entity)  
    {        await _dbSet.AddAsync(entity);  
        return entity;  
    }  
    public async Task<T> Update(T entity)  
    {        if (_context.Entry(entity).State == EntityState.Detached)  
        {            _dbSet.Attach(entity);  
        }        _context.Entry(entity).State = EntityState.Modified;  
        return entity;  
    }  
    public async Task<T> DeleteById(object id)  
    {        var entityToDelete = await _dbSet.FindAsync(id);  
        if (entityToDelete != null) _dbSet.Remove(entityToDelete);  
        return entityToDelete;  
    }  
    public async Task<T> GetByExpression(Expression<Func<T, bool>> filter,  
        params Expression<Func<T, object>>[] includeProperties)  
    {        IQueryable<T> query = _dbSet;  
  
        if (includeProperties != null)  
            foreach (var includeProperty in includeProperties)  
                query = query.Include(includeProperty);  
  
        return await query.SingleOrDefaultAsync(filter);  
    }  
    public async Task<List<T>> InsertRange(IEnumerable<T> entities)  
    {        _dbSet.AddRange(entities);  
        return entities.ToList();  
    }  
    public async Task<List<T>> DeleteRange(IEnumerable<T> entities)  
    {        _dbSet.RemoveRange(entities);  
        return entities.ToList();  
    }  
    public async Task<List<T>> UpdateRange(IEnumerable<T> entities)  
    {        foreach (var entity in entities)  
        {            if (_context.Entry(entity).State == EntityState.Detached)  
            {                _dbSet.Attach(entity);  
            }  
            // Mark the entity as modified  
            _context.Entry(entity).State = EntityState.Modified;  
        }        return entities.ToList();  
    }
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
    {
        return await _dbSet.AnyAsync(filter);
    }

    public async Task<List<Dish>> GetDishesWithIngredients(Expression<Func<Dish, bool>>? filter = null)
    {
        IQueryable<Dish> query = _context.Dishes
        .Include(d => d.Category) // Include category
        .Include(d => d.DishIngredients) // Include DishIngredients
        .ThenInclude(di => di.Ingredient); // Include Ingredient t? DishIngredients

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public IQueryable<T> GetQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public async Task DeleteWhere(Expression<Func<T, bool>> predicate)
    {
        var entities = _context.Set<T>().Where(predicate);
        _context.Set<T>().RemoveRange(entities);
        await _context.SaveChangesAsync(); 
    }


}