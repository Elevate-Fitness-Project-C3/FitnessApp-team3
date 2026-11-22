using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitnessProfileService.Features.OnBoarding.Set_training_preferences.Commands
{
    public class UpdateTrainingPreferencesCommandHandler(
        IGeneralRepository<FitnessProfile> repository)
        : IRequestHandler<UpdateTrainingPreferencesCommand, Unit>
    {
        public Task<Unit> Handle(
            UpdateTrainingPreferencesCommand request,
            CancellationToken cancellationToken)
        {
            var profile = new FitnessProfile
            {
                Id = request.ProfileId,
                UserId = request.UserId,
                TrainingExperienceLevel = request.TrainingExperienceLevel,
                AvailableDaysPerWeek = request.AvailableDaysPerWeek
            };

            repository.SaveInclude(
                profile,
                nameof(FitnessProfile.TrainingExperienceLevel),
                nameof(FitnessProfile.AvailableDaysPerWeek));

            return Task.FromResult(Unit.Value);
        }
    }
    }
