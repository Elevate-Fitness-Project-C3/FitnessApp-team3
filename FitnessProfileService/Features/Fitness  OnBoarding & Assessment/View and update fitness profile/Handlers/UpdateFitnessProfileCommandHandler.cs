
using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Commands
{
    public class UpdateFitnessProfileCommandHandler(
        IGeneralRepository<FitnessProfile> repository)
        : IRequestHandler<UpdateFitnessProfileCommand, Unit>
    {
        public async Task<Unit> Handle(
            UpdateFitnessProfileCommand request,
            CancellationToken cancellationToken)
        {
            var profile = await repository.GetAll()
                .FirstOrDefaultAsync(
                    x => x.UserId == request.UserId,
                    cancellationToken);

            if (profile is null)
                throw new NotFoundException("Fitness profile not found.");

            var updatedProperties = new List<string>();
            var model = request.ViewModel;

            if (model.Gender.HasValue &&
                profile.Gender != model.Gender.Value)
            {
                profile.Gender = model.Gender.Value;
                updatedProperties.Add(nameof(FitnessProfile.Gender));
            }

            if (model.DateOfBirth.HasValue &&
                profile.DateOfBirth != model.DateOfBirth.Value)
            {
                profile.DateOfBirth = model.DateOfBirth.Value;
                updatedProperties.Add(nameof(FitnessProfile.DateOfBirth));
            }

            if (model.PrimaryGoal.HasValue &&
                profile.PrimaryGoal != model.PrimaryGoal.Value)
            {
                profile.PrimaryGoal = model.PrimaryGoal.Value;
                updatedProperties.Add(nameof(FitnessProfile.PrimaryGoal));
            }

            if (model.PhysicalActivityLevel.HasValue &&
                profile.PhysicalActivityLevel != model.PhysicalActivityLevel.Value)
            {
                profile.PhysicalActivityLevel = model.PhysicalActivityLevel.Value;
                updatedProperties.Add(nameof(FitnessProfile.PhysicalActivityLevel));
            }

            if (model.TrainingExperienceLevel.HasValue &&
                profile.TrainingExperienceLevel != model.TrainingExperienceLevel.Value)
            {
                profile.TrainingExperienceLevel = model.TrainingExperienceLevel.Value;
                updatedProperties.Add(nameof(FitnessProfile.TrainingExperienceLevel));
            }

            if (model.AvailableDaysPerWeek.HasValue &&
                profile.AvailableDaysPerWeek != model.AvailableDaysPerWeek.Value)
            {
                profile.AvailableDaysPerWeek = model.AvailableDaysPerWeek.Value;
                updatedProperties.Add(nameof(FitnessProfile.AvailableDaysPerWeek));
            }

            if (updatedProperties.Count > 0)
            {
                profile.UpdatedAt = DateTime.UtcNow;

                updatedProperties.Add(nameof(FitnessProfile.UpdatedAt));

                repository.SaveInclude(
                    profile,
                    updatedProperties.ToArray());
            }

            return Unit.Value;
        }
    }
}
