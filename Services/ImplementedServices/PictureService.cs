using System;
using System.Collections.Generic;
using DAL;
using DAL.Entities;
using DAL.Repository;
using MimeKit;
using MongoDB.Bson;
using Services.Models.Request;
using Services.Models.Response;
using Services.Utils;

namespace Services.ImplementedServices
{
    public class PictureService : Service<Picture>
    {
        public PictureService(UnitOfWork uow, IRepository<Picture> repository, GlobalService service) : base(uow, repository, service){}

        public override Task<ItemResponseModel<Picture>> Create(Picture entity)
        {
            throw new NotImplementedException();
        }

        public override Task<ItemResponseModel<Picture>> Update(string id, Picture entity)
        {
            throw new NotImplementedException();
        }


        public async Task<ItemResponseModel<PictureResponse>> AddPicture(string aquarium, PictureRequest pictureRequest)
        {
            ItemResponseModel<PictureResponse> returnModel = new ItemResponseModel<PictureResponse>();
            returnModel.Data = null;
            returnModel.HasError = true;

            if(pictureRequest.FormFile != null)
            {
                string filename = pictureRequest.FormFile.FileName;

                if (!String.IsNullOrEmpty(filename))
                {
                    String typ = MimeTypes.GetMimeType(filename);
                    if (typ.StartsWith("image/"))
                    {
                        byte[] binaries = null;

                        using(var stream = new MemoryStream())
                        {
                            pictureRequest.FormFile.CopyTo(stream);
                            binaries = stream.ToArray();
                        }
                        ObjectId pictureId = await unitOfWork.Context.GridFSBucket.UploadFromBytesAsync(filename, binaries);

                        Picture pic = new Picture(pictureId.ToString(), aquarium, pictureRequest.Description, typ);

                        Picture savedPicture = await unitOfWork.Picture.InsertOneAsync(pic);
                        var bytes = await unitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(pictureId);

                        PictureResponse responsePicture = new PictureResponse();
                        responsePicture.Picture = savedPicture;
                        responsePicture.Bytes = bytes;
                        returnModel.Data = responsePicture;
                        returnModel.HasError = false;

                    }
                    else
                    {
                        returnModel.ErrorMessages.Add("Only images are allowed");
                    }
                }
                else
                {
                    returnModel.ErrorMessages.Add("Filename is empty");
                }
            }
            else
            {
                returnModel.ErrorMessages.Add("No Picture Provided");
            }

            return returnModel;
        }

        public async Task<ItemResponseModel<PictureResponse>> Get(string id)
        {
            ItemResponseModel<PictureResponse> result = new ItemResponseModel<PictureResponse>();

            if (id != null)
            {
                Picture picture = await repository.FindByIdAsync(id);

                var bytes = await unitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(picture.PictureID);

                PictureResponse response = new PictureResponse();
                response.Picture = picture;
                response.Bytes = bytes;
                result.Data = response;
            }
            else
            {
                result.HasError = true;
                modelStateWrapper.AddError("No Picture", "Picture was not found!");
            }
            return result;
        }

     

        public async Task<ActionResponseModel> Delete(string id)
        {
            ActionResponseModel deleted = new ActionResponseModel();

            if (!String.IsNullOrEmpty(id))
            {
                Picture foundPicture = await repository.FindByIdAsync(id);
                if (foundPicture != null)
                {
                    try
                    {
                        await unitOfWork.Context.GridFSBucket.DeleteAsync(new ObjectId(foundPicture.PictureID));
                        await repository.DeleteByIdAsync(id);

                        deleted.Success = true;
                    }
                    catch (Exception ex)
                    {
                        log.Warning(ex, "Delete Failed");
                        deleted.Success = false;
                        deleted.HasError = true;
                        deleted.ErrorMessages.Add("Delete Failed");
                    }

                }
                else
                {
                    deleted.Success = false;
                    deleted.HasError = true;
                    deleted.ErrorMessages.Add("Image not found");
                }

            }
            else
            {
                deleted.Success = false;
                deleted.HasError = true;
                deleted.ErrorMessages.Add("ID was empty");
            }

            return deleted;
        }


        public async Task<ItemResponseModel<List<PictureResponse>>> GetForAquarium(string aquarium)
        {

            ItemResponseModel<List<PictureResponse>> returnModel = new ItemResponseModel<List<PictureResponse>>();

            if (!String.IsNullOrEmpty(aquarium))
            {
                Aquarium foundAquarium = await unitOfWork.Aquarium.FindOneAsync(x => x.Name.Equals(aquarium));

                if(foundAquarium != null)
                {
                    List<PictureResponse> allPictureResponses = new List<PictureResponse>();
                    List<Picture> foundPictures = repository.FilterBy(x => x.Aquarium.Equals(foundAquarium.Name)).ToList();

                    foreach (Picture i in foundPictures)
                    {
                        var bytes = await unitOfWork.Context.GridFSBucket.DownloadAsBytesAsync(i.PictureID);
                        PictureResponse responsePicture = new PictureResponse();
                        responsePicture.Picture = i;
                        responsePicture.Bytes = bytes;
                        allPictureResponses.Add(responsePicture);
                    }
                    returnModel.Data = allPictureResponses;
                    returnModel.HasError = false;

                }
                else
                {
                    modelStateWrapper.AddError("No Aquarium", "Aquarium was not found!");

                }

            }
            else
            {
                modelStateWrapper.AddError("No Aquarium", "Aquarium Name was not provided!");

            }
            return returnModel;

        }
        public async override Task<bool> Validate(Picture entity)
        {
            if (entity != null)
            {

            }
            else
            {
                modelStateWrapper.AddError("Empty", "Picture is Empty");
            }
            return modelStateWrapper.IsValid;
        }
    }

}

