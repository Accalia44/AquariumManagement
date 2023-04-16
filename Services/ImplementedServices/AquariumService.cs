using DAL;
using DAL.Entities;
using DAL.Repository;
using Services.Models.Response;

namespace Services;

public class AquariumService : Service<Aquarium>
{
    public AquariumService(UnitOfWork uow, IRepository<Aquarium> repository, GlobalService service) : base(uow, repository, service)
    { }

    public override async Task<ItemResponseModel<Aquarium>> Create(Aquarium entity)
    {
        Console.WriteLine("hello");
        ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();
        response.Data = await repository.InsertOneAsync(entity);
        response.HasError = false;
        return response;
    }

    public override async Task<ItemResponseModel<Aquarium>> Update(string id, Aquarium entity)
    {
        ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();
        response.Data = await repository.UpdateOneAsync(entity);
        response.HasError = false;
        return response;
    }

    public async Task<ItemResponseModel<List<Aquarium>>> GetForUser(User user)
    {
        ItemResponseModel<List<Aquarium>> response = new ItemResponseModel<List<Aquarium>>();

        if (user != null)
        {
            var userAquarium = unitOfWork.UserAquarium.FilterBy(x => x.UserID == user.ID);

            var aquarium = unitOfWork.Aquarium.FilterBy(a => userAquarium.Any(usr => usr.AquariumID == a.ID));

            response.Data = aquarium.ToList();
            response.HasError = false;
        }
        else
        {
            response.HasError = true;
            response.ErrorMessages.Add("No User provided.");
        }
        return response;
    }

    public override async Task<Aquarium> Get(string id)
    {
        Aquarium found = await this.repository.FindByIdAsync(id);
        if(!String.IsNullOrEmpty(found.ID))
        {

        }
        else
        {
            modelStateWrapper.AddError("No Aquarium found", "Please provide an existing Aquarium");
        }
        return found;

    }

    public override async Task<bool> Validate(Aquarium entity)
    {
        if (entity != null)
        {
            var searchedAquarium = await repository.FindOneAsync(x => x.Name.Equals(entity.Name));

            if (!String.IsNullOrEmpty(searchedAquarium.Name))
            {
                modelStateWrapper.AddError("Aquarium Exists", "Pleaes use a different Name, this name already exists");
            }

            if (String.IsNullOrEmpty(entity.Name))
            {
                modelStateWrapper.AddError("No Name", "Please provide a name");
            }

            if (entity.Depth <= 0)
            {
                modelStateWrapper.AddError("No Depth", "Please provide a depth bigger than 0");
            }

            if (entity.Length <= 0)
            {
                modelStateWrapper.AddError("No Length", "Please provide a length longer than 0");
            }

            if (entity.Height <= 0)
            {
                modelStateWrapper.AddError("No Heigth", "Please provide a height higher then 0");
            }

            if (entity.Liters <= 0)
            {
                modelStateWrapper.AddError("No Liters", "Please provide liters more then 0");
            }
        }
        else
        {
            modelStateWrapper.AddError("No Aquarium", "Please provide an aquarium");
        }
        return modelStateWrapper.IsValid;
    }


}

