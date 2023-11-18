using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Domain.Entities;
using StudentInformationSystem.Persistence.Interfaces.Repository.RoleRepository;

namespace StudentInformationSystem.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public UserRoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<UserRole> GetOrCreateRoleAsync(string roleName)
        {
            var existingRole = await GetByNameAsync(roleName);
            if (existingRole != null)
            {
                return existingRole;
            }

            var newRole = new UserRole
            {
                RoleName = roleName
            };

            await _roleRepository.AddAsync(newRole);

            return newRole;
        }
        private async Task<UserRole> GetByNameAsync (string roleName)
        {
            var allRole = await _roleRepository.GetFilterAsync(x=>x.RoleName == roleName);
            return allRole;
        }
        public async Task<UserRole> GetByRoleAsync(int roleId)
        {
            var role = await _roleRepository.GetFilterAsync(x => x.Id.Equals(roleId) && !x.IsDeleted);
            return role;
        }
    }
}
