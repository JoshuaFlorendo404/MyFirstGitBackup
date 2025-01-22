using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactDB_OOP
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        public CompanyRepository(MyCon dbConnection) : base(dbConnection) { }

        private async Task<DbCommand> CreateCommandAsync(string query, params MySqlParameter[] parameters)
        {
            var connection = _dbConnection.GetConnection();
            var command = connection.CreateCommand();
            command.CommandText = query;

            if (parameters != null && parameters.Length > 0)
            {
                command.Parameters.AddRange(parameters);
            }

            await connection.OpenAsync();
            return command;
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            const string query = "SELECT company_id, company_name FROM tbl_company";
            var companies = new List<Company>();

            using (var command = await CreateCommandAsync(query))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    companies.Add(new Company
                    {
                        company_id = reader.GetInt32(0),
                        company_name = reader.GetString(1)
                    });
                }
            }

            return companies;
        }

        public async Task<Company> GetCompanyByIdAsync(int companyId)
        {
            const string query = "SELECT company_id, company_name FROM tbl_company WHERE company_id = @CompanyId";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@CompanyId", companyId)))
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new Company
                    {
                        company_id = reader.GetInt32(0),
                        company_name = reader.GetString(1)
                    };
                }
            }

            return null;
        }

        public async Task<bool> AddCompanyAsync(string companyName)
        {
            const string query = "INSERT INTO tbl_company (company_name) VALUES (@CompanyName)";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@CompanyName", companyName)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> UpdateCompanyAsync(Company company)
        {
            const string query = "UPDATE tbl_company SET company_name = @CompanyName WHERE company_id = @CompanyId";

            using (var command = await CreateCommandAsync(query,
                new MySqlParameter("@CompanyName", company.company_name),
                new MySqlParameter("@CompanyId", company.company_id)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> DeleteCompanyAsync(int companyId)
        {
            const string query = "DELETE FROM tbl_company WHERE company_id = @CompanyId";

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@CompanyId", companyId)))
            {
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<IEnumerable<Company>> SearchCompaniesByNameAsync(string companyName)
        {
            const string query = "SELECT company_id, company_name FROM tbl_company WHERE company_name LIKE @CompanyName";
            var companies = new List<Company>();

            using (var command = await CreateCommandAsync(query, new MySqlParameter("@CompanyName", $"%{companyName}%")))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    companies.Add(new Company
                    {
                        company_id = reader.GetInt32(0),
                        company_name = reader.GetString(1)
                    });
                }
            }

            return companies;
        }
    }
}
