using System;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Services.Models.Request;
using Services.Models.Response;

namespace Services.ImplementedServices
{
    public class PictureService : Service<Picture>
    {
        public PictureService(UnitOfWork uow, IRepository<Picture> repository, GlobalService service) : base(uow, repository, service)
        {
        }

        public override Task<ItemResponseModel<Picture>> Create(Picture entity)
        {
            throw new NotImplementedException();
        }

        public override Task<ItemResponseModel<Picture>> Update(string id, Picture entity)
        {
            throw new NotImplementedException();
        }

        public async override Task<bool> Validate(Picture entity)
        {
            if(entity != null)
            {

            }
            else
            {
                modelStateWrapper.AddError("Empty", "Picture is Empty");
            }
            return modelStateWrapper.IsValid;
        }
        public async Task<ItemResponseModel<PictureResponse>> AddPicture(string aquarium, PictureRequest pictureRequest)
        {
            ItemResponseModel<PictureResponse> returnModel = new ItemResponseModel<PictureResponse>();
            returnModel.Data = null;
            returnModel.HasError = true;

            return returnModel;
        }
    }
}

