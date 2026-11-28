using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Domain.Entities;
using FitnessProfileService.Domain.Repositories;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Dtos;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Handlers
{
    public class GetFitnessProfileQueryHandler(
        IGeneralRepository<FitnessProfile> repository)
        : IRequestHandler<GetFitnessProfileQuery, FitnessProfileDto>
    {
        public async Task<FitnessProfileDto> Handle(
            GetFitnessProfileQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await repository.GetAll()
                .Where(x => x.UserId == request.UserId)
                .Select(x => new FitnessProfileDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth,
                    PrimaryGoal = x.PrimaryGoal,
                    PhysicalActivityLevel = x.PhysicalActivityLevel,
                    TrainingExperienceLevel = x.TrainingExperienceLevel,
                    AvailableDaysPerWeek = x.AvailableDaysPerWeek,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (profile is null)
                throw new NotFoundException("Fitness profile not found.");

            return profile;
        }
    }    
    }

