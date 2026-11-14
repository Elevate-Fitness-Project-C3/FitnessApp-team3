    namespace IdentityService.Common.Middleware;

public class NotFoundException(string message) : Exception(message);
public class ConflictException(string message) : Exception(message);
public class GoneException(string message) : Exception(message);
