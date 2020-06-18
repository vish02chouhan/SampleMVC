using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.General.Interfaces;

namespace Infrastructure.General.Implementation
{
    /// <summary>
    /// Database Repository
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private IDapperContext _context;

        public BaseRepository(IDapperContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Connection = Context.Connection;
        }

        private IDbConnection Connection { get; }

        public IDapperContext Context
        {
            get
            {
                if (_context == null)
                {
                    throw new Exception("context has not been set!");
                }

                return _context;
            }

            set => _context = value;
        }

        #region Upserts

        public async Task<T> UpsertAsync(string storedProcedure, object dynamicParameters)
        {
            //FOLLOWS SAME PROCESS AS INSERT - ADDED THIS TASK FOR CLARITY OF PURPOSE TO CALLING PROCESSES
            return
                await
                    Context.WithConnection(async c =>
                        await
                            c.QueryFirstOrDefaultAsync<T>(
                                storedProcedure,
                                dynamicParameters,
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: Context.TimeoutPeriod
                                )
                        );
        }

        #endregion

        #region Gets

        public async Task<T> GetByIdAsync(string id, string storedProcedure)
        {
            return await Context.WithConnection(async c =>
                await c.QueryFirstAsync<T>(
                    storedProcedure,
                    new { Id = id },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod
                    )
                );
        }

        public async Task<T> GetScalar(string sqlQuery, object dynamicParameters)
        {
            return await Context.WithConnection(async c =>
                await c.QueryFirstOrDefaultAsync<T>(
                    sqlQuery,
                    dynamicParameters,
                    commandType: CommandType.Text,
                    commandTimeout: Context.TimeoutPeriod
                    )
                );
        }

        public async Task<object> GetScalarObject(object dynamicParameters, string storedProcedure)
        {
            return await Context.WithConnection(async c =>
                  await c.ExecuteScalarAsync(
                  storedProcedure,
                  dynamicParameters,
                  commandType: CommandType.StoredProcedure,
                  commandTimeout: Context.TimeoutPeriod
                  )
                );
        }

        public async Task<T> GetByDynamicAsync(object dynamicParameters, string storedProcedure)
        {
            return await Context.WithConnection(async c =>
                await c.QueryFirstOrDefaultAsync<T>(
                    storedProcedure,
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod
                    )
                );
        }

        public IEnumerable<T> GetAll(string storedProcedure)
        {
            IEnumerable<T> result;

            using (var localContext = Context)
            {
                // We pass procs like these null guid's to get all.
                result = localContext.Connection.Query<T>(storedProcedure, new { Id = (Guid?)null },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod);
            }

            return result;
        }

        public IEnumerable<T> GetAll(object dynamicParameters, string storedProcedure)
        {
            IEnumerable<T> result;

            using (var localContext = Context)
            {
                // We pass procs like these dynamic parameters
                result = localContext.Connection.Query<T>(storedProcedure, dynamicParameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod);
            }

            return result;
        }

        public object GetAllMultipleQuery(object dynamicParameters, string storedProcedure, IDapperContext context)
        {
            SqlMapper.GridReader reader;
            reader = context.Connection.QueryMultiple(storedProcedure, dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod);
            return reader;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string storedProcedure)
        {
            return await Context.WithConnection(async c =>
                await c.QueryAsync<T>(
                    storedProcedure,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod
                    )
                );
        }

        public async Task<IEnumerable<T>> GetAllAsync(object dynamicParameters, string storedProcedure)
        {
            return await Context.WithConnection(async c =>
                await c.QueryAsync<T>(
                    storedProcedure,
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod
                    )
                );
        }

        public T GetById(Guid id, string storedProcedure)
        {
            T results;

            using (var localContext = Context)
            {
                results =
                    localContext.Connection.Query<T>(storedProcedure, new { search_query_id = id },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod)
                        .SingleOrDefault();
            }

            return results;
        }

        public T GetById(int id, string storedProcedure)
        {
            T results;

            using (var localContext = Context)
            {
                results =
                    localContext.Connection.Query<T>(storedProcedure, new { Id = id },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod)
                        .SingleOrDefault();
            }

            return results;
        }

        public T GetById(string id, string storedProcedure)
        {
            T results;

            using (var localContext = Context)
            {
                results =
                    localContext.Connection.Query<T>(storedProcedure, new { Id = id },
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod)
                        .SingleOrDefault();
            }

            return results;
        }

        public T GetByDynamic(object dynamicParameters, string storedProcedure)
        {
            T results;

            using (var localContext = Context)
            {
                results =
                    localContext.Connection.Query<T>(storedProcedure, dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod)
                        .SingleOrDefault();
            }

            return results;
        }

        protected IEnumerable<T> GetAllDynamic(string storedProcedure)
        {
            List<T> results;

            using (var localContext = Context)
            {
                results = localContext.Connection.Query<T>(storedProcedure,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod).ToList();
            }

            return results;
        }

        public object GetByDynamic(string storedProcedure, object dynamicParameters)
        {
            object result;

            using (var localContext = Context)
            {
                result = localContext.Connection.ExecuteScalar(storedProcedure, dynamicParameters,
                    commandTimeout: Context.TimeoutPeriod,
                    commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        #endregion

        #region Inserts

        /// <summary>
        ///     Generic insert
        /// </summary>
        public T Insert(T entity, string storedProcedure)
        {
            T results;

            using (var localContext = Context)
            {
                results =
                    localContext.Connection.Query<T>(storedProcedure, entity, commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod)
                        .SingleOrDefault();
            }

            return results;
        }

        /// <summary>
        ///     Generic insert with dynamic parameters
        /// </summary>
        public T Insert(T entity, string storedProcedure, object dynamicParameters)
        {
            T results;

            using (var localContext = Context)
            {
                results =
                    localContext.Connection.Query<T>(storedProcedure, dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod)
                        .SingleOrDefault();
            }

            return results;
        }

        /// <summary>
        ///     Async insert with dynamic parameters which does not return data
        /// </summary>
        public async Task InsertAsync(string storedProcedure, object dynamicParameters)
        {
            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();

            try
            {
                using (scope)
                {
                    await Task.Run(() => scope.Connection.Execute(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope,
                        commandTimeout: scope.Connection.ConnectionTimeout));

                    // End scope
                    scope.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);

                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }
        }

        /// <summary>
        ///     Async insert with dynamic parameters which return inserted object
        /// </summary>
        public async Task<T> InsertAsync(object dynamicParameters, string storedProcedure)
        {
            return
                await
                    Context.WithConnection(async c =>
                        await
                            c.QueryFirstOrDefaultAsync<T>(
                                storedProcedure,
                                dynamicParameters,
                                commandType: CommandType.StoredProcedure,
                                commandTimeout: Context.TimeoutPeriod
                                )
                        );
        }

        /// <summary>
        ///     Generic insert in a scope of a transaction
        /// </summary>
        public T Insert(IDbConnection connection, T entity, string storedProcedure, IDbTransaction transaction)
        {
            return
                connection.Query<T>(
                    storedProcedure, entity,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction,
                    commandTimeout: Context.TimeoutPeriod)
                    .SingleOrDefault();
        }

        /// <summary>
        ///     Generic insert with dynamic parameters in the scope of a transaction
        /// </summary>
        public T Insert(IDbConnection connection, T entity, string storedProcedure, object dynamicParameters,
            IDbTransaction transaction)
        {
            return
                connection.Query<T>(storedProcedure, dynamicParameters, commandType: CommandType.StoredProcedure,
                    transaction: transaction, commandTimeout: Context.TimeoutPeriod).SingleOrDefault();
        }

        /// <summary>
        ///     Generic insert with dynamic parameters
        /// </summary>
        /// <returns>A list of the inserted objects</returns>
        public IEnumerable<T> Insert(object dynamicParameters, string storedProcedure)
        {
            IEnumerable<T> results;

            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();

            try
            {
                using (scope)
                {
                    results = Connection.Query<T>(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope,
                        commandTimeout: Context.TimeoutPeriod);

                    // End scope
                    scope.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);
                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }

            return results;
        }

        public object Insert(string storedProcedure, object dynamicParameters)
        {

            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();
            object result;
            try
            {
                using (scope)
                {
                    var dataRow = Connection.Query(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope,
                        commandTimeout: Context.TimeoutPeriod).FirstOrDefault();

                    // End scope
                    scope.Commit();
                    result = ((object[])((System.Collections.Generic.IDictionary<string, object>)dataRow).Values)[0];
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);
                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }

            return result;
        }
        #endregion

        #region Updates

        /// <summary>
        ///     Generic update
        /// </summary>
        public T Update(T entity, string storedProcedure)
        {
            T results;

            using (var localContext = Context)
            {
                results =
                    localContext.Connection.Query<T>(storedProcedure, entity, commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod)
                        .SingleOrDefault();
            }

            return results;
        }

        /// <summary>
        ///     Generic update in the scope of a transaction
        /// </summary>
        public T Update(IDbConnection connection, T entity, string storedProcedure, IDbTransaction transaction)
        {
            return
                connection.Query<T>(storedProcedure, entity, commandType: CommandType.StoredProcedure,
                    transaction: transaction, commandTimeout: Context.TimeoutPeriod)
                    .SingleOrDefault();
        }

        /// <summary>
        ///     Generic update with dynamic parameters
        /// </summary>
        public T Update(T entity, string storedProcedure, object dynamicParameters)
        {
            T results;

            using (var localContext = Context)
            {
                results =
                    localContext.Connection.Query<T>(storedProcedure, dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: Context.TimeoutPeriod)
                        .SingleOrDefault();
            }

            return results;
        }

        /// <summary>
        ///     Generic update with dynamic parameters
        /// </summary>
        /// <returns>A list of the updated objects</returns>
        public IEnumerable<T> Update(object dynamicParameters, string storedProcedure)
        {
            IEnumerable<T> results;

            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();

            try
            {
                using (scope)
                {
                    results = Connection.Query<T>(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope, commandTimeout: Context.TimeoutPeriod);

                    // End scope
                    scope.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);

                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }

            return results;
        }

        /// <summary>
        ///     Generic update with dynamic parameters in the scope of a transaction
        /// </summary>
        public T Update(IDbConnection connection, T entity, string storedProcedure, object dynamicParameters,
            IDbTransaction transaction)
        {
            return
                connection.Query<T>(storedProcedure, dynamicParameters, commandType: CommandType.StoredProcedure,
                    transaction: transaction, commandTimeout: Context.TimeoutPeriod).SingleOrDefault();
        }

        /// <summary>
        ///     Generic Update with dynamic parameters which does not return data
        /// </summary>
        public void Update(string storedProcedure, object dynamicParameters)
        {
            using (var localContext = Context)
            {
                localContext.Connection.Execute(storedProcedure, dynamicParameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod);
            }
        }

        /// <summary>
        ///     Async Update with dynamic parameters which does not return data
        /// </summary>
        public async Task UpdateAsync(string storedProcedure, object dynamicParameters)
        {
            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();

            try
            {
                using (scope)
                {
                    await Task.Run(() => scope.Connection.Execute(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope,
                        commandTimeout: scope.Connection.ConnectionTimeout));

                    // End scope
                    scope.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);

                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }
        }

        /// <summary>
        ///     Async update with dynamic parameters which return updated object
        /// </summary>
        public async Task<T> UpdateAsync(object dynamicParameters, string storedProcedure)
        {
            return
               await
                   Context.WithConnection(async c =>
                       await
                           c.QueryFirstOrDefaultAsync<T>(
                               storedProcedure,
                               dynamicParameters,
                               commandType: CommandType.StoredProcedure,
                               commandTimeout: Context.TimeoutPeriod
                               )
                       );
        }

        #endregion

        #region Deletes

        /// <summary>
        ///     Generic delete
        /// </summary>
        public void Delete(T entity, string storedProcedure)
        {
            using (var localContext = Context)
            {
                localContext.Connection.Execute(storedProcedure, entity, commandTimeout: Context.TimeoutPeriod);
            }
        }

        /// <summary>
        ///     Generic delete in the scope of a transaction
        /// </summary>
        public void Delete(IDbConnection connection, T entity, string storedProcedure, IDbTransaction transaction)
        {
            connection.Execute(storedProcedure, entity, transaction, Context.TimeoutPeriod);
        }

        /// <summary>
        ///     Generic delete with uniqueidentifier as the id
        /// </summary>
        public void Delete(Guid id, string storedProcedure)
        {
            using (var localContext = Context)
            {
                localContext.Connection.Execute(storedProcedure, new { Id = id }, commandType: CommandType.StoredProcedure,
                    commandTimeout: Context.TimeoutPeriod);
            }
        }

        /// <summary>
        ///     Generic delete with dynamic parameters
        /// </summary>
        public void Delete(string storedProcedure, object dynamicParameters)
        {
            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();

            try
            {
                using (scope)
                {
                    scope.Connection.Execute(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope,
                        commandTimeout: Context.TimeoutPeriod);

                    // End scope
                    scope.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);
                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }
        }

        /// <summary>
        ///     Async Delete with dynamic parameters which does not return data
        /// </summary>
        public async Task DeleteAsync(string storedProcedure, object dynamicParameters)
        {
            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();

            try
            {
                using (scope)
                {
                    await Task.Run(() => scope.Connection.Execute(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope,
                        commandTimeout: scope.Connection.ConnectionTimeout));

                    // End scope
                    scope.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);
                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }
        }

        /// <summary>
        ///     Generic delete with dynamic parameters
        /// </summary>
        /// <returns>A list of the deleted objects</returns>
        public IEnumerable<T> Delete(object dynamicParameters, string storedProcedure)
        {
            IEnumerable<T> results;

            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();

            try
            {
                using (scope)
                {
                    results = Connection.Query<T>(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope,
                        commandTimeout: Context.TimeoutPeriod);

                    // End scope
                    scope.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);
                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }

            return results;
        }

        /// <summary>
        ///     Generic delete with uniqueidentifier as the id, in the scope of a transaction
        /// </summary>
        public void Delete(IDbConnection connection, Guid id, string storedProcedure, IDbTransaction transaction)
        {
            connection.Execute(storedProcedure, new { Id = id }, commandType: CommandType.StoredProcedure,
                transaction: transaction, commandTimeout: Context.TimeoutPeriod);
        }

        /// <summary>
        ///     Generic delete with dynamic parameters in the scope of a transaction
        /// </summary>
        public void Delete(IDbConnection connection, object dynamicParameters, string storedProcedure,
            IDbTransaction transaction)
        {
            connection.Execute(storedProcedure, dynamicParameters, commandType: CommandType.StoredProcedure,
                transaction: transaction, commandTimeout: Context.TimeoutPeriod);
        }

        /// <summary>
        ///     Async Delete with dynamic parameters which return deleted object
        /// </summary>
        public async Task<T> DeleteAsync(object dynamicParameters, string storedProcedure)
        {
            return
               await
                   Context.WithConnection(async c =>
                       await
                           c.QueryFirstOrDefaultAsync<T>(
                               storedProcedure,
                               dynamicParameters,
                               commandType: CommandType.StoredProcedure,
                               commandTimeout: Context.TimeoutPeriod
                               )
                       );
        }

        #endregion

        #region Execute

        /// <summary>
        ///     Generic execute with dynamic parameters
        /// </summary>
        public void Execute(string storedProcedure, object dynamicParameters)
        {
            // Start a new transaction scope
            var scope = Context.Connection.BeginTransaction();

            try
            {
                using (scope)
                {
                    Context.Connection.Execute(
                        storedProcedure,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure,
                        transaction: scope,
                        commandTimeout: Context.TimeoutPeriod);

                    // End scope
                    scope.Commit();
                }
            }
            catch (Exception)
            {
                //TODO: Uncomment while setting up Elmah
                //ErrorSignal.FromCurrentContext().Raise(ex);
                // Rollback scope if any error is encountered
                scope.Rollback();

                // Rethrow the last exception
                throw;
            }
            finally
            {
                Context.Dispose();
            }
        }

        #endregion
    }
}
