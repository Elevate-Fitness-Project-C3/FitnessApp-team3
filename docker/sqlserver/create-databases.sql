IF DB_ID(N'FitnessIdentity') IS NULL
    CREATE DATABASE [FitnessIdentity];
GO

IF DB_ID(N'FitnessProfile') IS NULL
    CREATE DATABASE [FitnessProfile];
GO

IF DB_ID(N'FitnessTraining') IS NULL
    CREATE DATABASE [FitnessTraining];
GO

IF DB_ID(N'FitnessProgress') IS NULL
    CREATE DATABASE [FitnessProgress];
GO

IF DB_ID(N'FitnessNutrition') IS NULL
    CREATE DATABASE [FitnessNutrition];
GO

IF DB_ID(N'FitnessPayment') IS NULL
    CREATE DATABASE [FitnessPayment];
GO
