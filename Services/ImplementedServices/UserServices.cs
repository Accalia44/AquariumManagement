using System;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Microsoft.IdentityModel.Tokens;
using Services.Authentications;
using Services.Models.Request;
using Services.Models.Response;

namespace Services
{
    public class UserServices : Service<User>
    {
        public UserServices(UnitOfWork uow, IRepository<User> repository, GlobalService service) : base(uow, repository, service){}

        public async override Task<ItemResponseModel<User>> Create(User entity)
        {
            ItemResponseModel<User> response = new ItemResponseModel<User>();
            response.Data = await this.unitOfWork.User.SignUp(entity);
            response.HasError = false;
            return response;
        }

        public async override Task<ItemResponseModel<User>> Update(string id, User entity)
        {
            ItemResponseModel<User> response = new ItemResponseModel<User>();

            var foundUser = await this.repository.FindByIdAsync(entity.ID);
            if (foundUser != null)
            {
                response.Data = entity;
                response.HasError = false;
                return response;
            }

            else
            {
                modelStateWrapper.AddError("User Not Found", "User was not in Database");
                return response;
            }
            
        }

        public override async Task<bool> Validate(User entity)
        {
            if(entity != null)
            {
                if (String.IsNullOrEmpty(entity.Email))
                {
                    modelStateWrapper.AddError("No Email", "Please provide an Email");
                }
                else
                {
                    User usr = await this.repository.FindOneAsync(a => a.Email.Equals(entity.Email));

                    if(usr != null)
                    {
                        if (!String.IsNullOrEmpty(entity.ID))
                        {
                            if (entity.ID.Equals(usr.ID) == false)
                            {
                                modelStateWrapper.AddError("No Email", "Email already taken");
                            }
                        }
                    }
                }
                if (String.IsNullOrEmpty(entity.Firstname))
                {
                    modelStateWrapper.AddError("No Firstname", "Please provide a Firstname");
                }
                if (String.IsNullOrEmpty(entity.Lastname))
                {
                    modelStateWrapper.AddError("No Lastname", "Please provide a Lastname");
                }
            }
            else
            {
                modelStateWrapper.AddError("No User", "No user was provided");

            }
            return modelStateWrapper.IsValid;
        }
        
        public async Task<ItemResponseModel<UserResponse>> Login(LoginRequest request)
        {
            UnitOfWork uow = new UnitOfWork();

            UserResponse userResponse = new UserResponse(); 
            ItemResponseModel<UserResponse> response = new ItemResponseModel<UserResponse>();
            User testUserLogin = await this.unitOfWork.User.Login(request.Email, request.Password);

            userResponse.User = testUserLogin;

            Authentication auth = new Authentication(uow);
            AuthenticationInformation info = await auth.Authenticate(testUserLogin);

            userResponse.AuthenticationInformation = info;

            response.Data = userResponse;
            response.HasError = false;
            return response;

        }

    }

}

