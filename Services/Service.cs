﻿using System;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Services.Models.Response;
using Services.Utils;
using Utils;

namespace Services
{
	public abstract class Service<TEntity> where TEntity : Entity
	{
		protected UnitOfWork unitOfWork;
		protected IRepository<TEntity> repository; 
		protected GlobalService globalService;
        protected IModelStateWrapper modelStateWrapper;
        protected ModelStateDictionary Validation = null;
		protected User CurrentUser { get; private set; }
		protected Serilog.ILogger log = Logger.ContextLog<Service<TEntity>>();

		public Service(UnitOfWork uow, IRepository<TEntity> repository, GlobalService service)
		{
			unitOfWork = uow;
			this.repository = repository;
			this.globalService = service;
		}

		public virtual async Task<ActionResponseModel> Delete(string id)
		{
			await repository.DeleteByIdAsync(id);
			ActionResponseModel returnval = new ActionResponseModel();
			returnval.Success = true;

			return returnval;
		}

		public async Task Load(string email)
		{
			CurrentUser = await unitOfWork.User.FindOneAsync(x => x.Email.ToLower().Equals(email.ToLower()));
		}

		public abstract Task<ItemResponseModel<TEntity>> Create(TEntity entity);

		public abstract Task<ItemResponseModel<TEntity>> Update(string id, TEntity entity);

		public abstract Task<Boolean> Validate(TEntity entity);

		public virtual async Task<ItemResponseModel<TEntity>> CreateHandler(TEntity entity)
		{
			ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();

			if(await Validate(entity))
			{
				ItemResponseModel<TEntity> ent = await Create(entity);
				if (ent != null)
				{
					return ent;
				}
				else
				{
					response.HasError = true;
					response.Data = null; 
					response.ErrorMessages.Add("Object was emtpy");
				}
			}
			
			else
			{
				response.HasError = true;
				response.ErrorMessages = modelStateWrapper.Errors.Values.ToList();
			}
			return response;
		}

        public virtual async Task<ItemResponseModel<TEntity>> UpdateHandler(string id, TEntity entity)
        {
            ItemResponseModel<TEntity> response = new ItemResponseModel<TEntity>();

            if (await Validate(entity))
            {
                ItemResponseModel<TEntity> ent = await Update(id, entity);
                if (ent != null)
                {
                    if(ent.HasError == false)
					{
						ent.Data.ID = id;
						await this.repository.UpdateOneAsync(ent.Data);
						response.Data = ent.Data;
						response.HasError = false;

					}
					else
					{
						return ent; 
					}
                }
                else
                {
                    response.HasError = true;
                    response.Data = null;
                    response.ErrorMessages.Add("Object was emtpy");
                }
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages = modelStateWrapper.Errors.Values.ToList();
            }
            return response;
        }

		public async virtual Task<TEntity> Get(string id)
		{
			return await this.repository.FindByIdAsync(id);
		}

		public async virtual Task<List<TEntity>> Get()
		{
			return this.repository.FilterBy(x => true).ToList();
		}

        public async Task SetModelState(ModelStateDictionary validate)
		{
			modelStateWrapper = new ModelStateWrapper(validate);
			this.Validation = validate;
		}
	}
}

