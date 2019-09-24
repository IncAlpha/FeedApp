using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedIt.Data.Context;
using FeedIt.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedIt.Data.Repositories
{
    public abstract class BaseRepository<TModel>
        where TModel : BaseModel
    {
        protected readonly AppDbContext Context;

        public abstract DbSet<TModel> DbSet { get; }

        /// <summary>
        /// Returns all models;
        /// </summary>
        public IEnumerable<TModel> Entries => DbSet.AsEnumerable();

        protected BaseRepository(AppDbContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Is model exist in database;
        /// </summary>
        /// <param name="model">Target model object</param>
        public async Task<bool> IsExist(TModel model)
        {
            return await DbSet.AnyAsync(entry => entry.Id == model.Id);
        }

        /// <summary>
        /// Is model exist in database;
        /// </summary>
        /// <param name="id">Target model id</param>
        public async Task<bool> IsExist(Guid id)
        {
            return await DbSet.AnyAsync(entry => entry.Id == id);
        }

        /// <summary>
        /// Get model object by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Found model or null</returns>
        public async Task<TModel> Get(Guid id)
        {
            var result = await DbSet.FirstOrDefaultAsync(model => model.Id == id);
            result.IsExist = true;
            return result;
        }

        public async Task<TModel> Get(string id)
        {
            if (!Guid.TryParse(id, out var targetId)) return null;

            var result = await DbSet.FirstOrDefaultAsync(model => model.Id == targetId);
            result.IsExist = true;
            return result;
        }

        public async Task Save(TModel model)
        {
            var exist = await IsExist(model);
            Context.Entry(model).State = !exist ? EntityState.Added : EntityState.Modified;

            await Context.SaveChangesAsync();
        }

        public async Task Delete(TModel model)
        {
            DbSet.Remove(model);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var model = await Get(id);
            await Delete(model);
        }
    }
}