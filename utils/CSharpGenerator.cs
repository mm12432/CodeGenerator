using System.Text;
using code_generator.models;

namespace code_generator.utils
{
    public class CSharpGenerator
    {
        public static string MODELS = "Models";
        public static string VIEWMODELS = "ViewModels";
        public static string REPOSITORIES = "Repositories";
        public static string INTERFACES = "Interfaces";
        public static string CONTROLLERS = "Controllers";
        public static string APPCONTEXT = "AppContext";
        public static string COMMON = "Common";
        public static string SERVICES = "Services";

        private static string GetModelText(ModelDeclare model, string namespaceText, string modelOrViewModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine(string.Format(@"using {0}.{1};", namespaceText, COMMON));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}", namespaceText, modelOrViewModel));
            sb.AppendLine(@"{");
            sb.AppendLine(string.Format(@"    public class {0} : GuidEntity", model.Name));
            sb.AppendLine(@"    {");
            foreach (var prop in model.ModelProperties)
            {
                sb.AppendLine(string.Format(@"        public {0} {1} {{ get; set; }}", TypeConveror.ToCSharpType(prop.ValueType), prop.Name));
            }
            //sb.AppendLine(TypeConveror.ToCSharpType())
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateModelText(ModelDeclare model, string namespaceText)
        {
            return GetModelText(model, namespaceText, MODELS);
        }

        public static string GenerateViewModelText(ModelDeclare model, string namespaceText)
        {
            return GetModelText(model, namespaceText, VIEWMODELS);
        }

        public static string GenerateRepositoryText(string modelName, string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(@"using {0}.{1};", namespaceText, MODELS));
            sb.AppendLine(string.Format(@"using {0}.{1}.{2};", namespaceText, REPOSITORIES, INTERFACES));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}", namespaceText, REPOSITORIES));
            sb.AppendLine(@"{");
            sb.AppendLine(string.Format(@"    public class {0}Repository", modelName));
            sb.AppendLine(string.Format(@"        : BaseRepository<{0}>, I{0}Repository", modelName));
            sb.AppendLine(@"    {");
            sb.AppendLine(string.Format(@"        public {0}Repository({1} context)", modelName, APPCONTEXT));
            sb.AppendLine(string.Format(@"            : base(context)", modelName));
            sb.AppendLine(@"        {");
            sb.AppendLine(@"        }");
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateRepositoryInterfaceText(string modelName, string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine(string.Format(@"using {0}.{1};", namespaceText, MODELS));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}.{2}", namespaceText, REPOSITORIES, INTERFACES));
            sb.AppendLine(@"{");
            sb.AppendLine(string.Format(@"    public interface I{0}Repository", modelName));
            sb.AppendLine(string.Format(@"        : IBaseRepository<{0}>", modelName));
            sb.AppendLine(@"    {");
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateServiceText(string modelName, string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine(@"using AutoMapper;");
            sb.AppendLine(string.Format(@"using {0}.{1}.{2};", namespaceText, REPOSITORIES, INTERFACES));
            sb.AppendLine(string.Format(@"using {0}.{1}.{2};", namespaceText, SERVICES, INTERFACES));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}", namespaceText, SERVICES));
            sb.AppendLine(@"{");
            sb.AppendLine(string.Format(@"    public class {0}Service : I{0}Service", modelName));
            sb.AppendLine(@"    {");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        protected {0} _context;", APPCONTEXT));
            sb.AppendLine(string.Format(@"        protected I{0}Repository _{1}Repository;", modelName, modelName.ToLower()));
            sb.AppendLine(@"        protected IMapper _mapper;");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        public {0}Service({2} context, IMapper mapper, I{0}Repository {1}Repository)", modelName, modelName.ToLower(), APPCONTEXT));
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            this._context = context;");
            sb.AppendLine(@"            this._mapper = mapper;");
            sb.AppendLine(string.Format(@"            this._{0}Repository = {0}Repository;", modelName.ToLower()));
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        public void Add(ViewModels.{0} viewModel)", modelName));
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            Guid id = Guid.NewGuid();");
            sb.AppendLine(string.Format(@"            var model = this._mapper.Map<Models.{0}>(viewModel);", modelName));
            sb.AppendLine(@"            model.Id = id;");
            sb.AppendLine(string.Format(@"            this._{0}Repository.Add(model);", modelName.ToLower()));
            sb.AppendLine(@"            this._context.SaveChanges();");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        public void Update(Guid id, ViewModels.{0} viewModel)", modelName));
            sb.AppendLine(@"        {");
            sb.AppendLine(string.Format(@"            var model = this._{0}Repository.Get(id);", modelName.ToLower()));
            sb.AppendLine(string.Format(@"            this._mapper.Map<ViewModels.{0}, Models.{0}>(viewModel, model);", modelName));
            sb.AppendLine(string.Format(@"            this._{0}Repository.Update(model);", modelName.ToLower()));
            sb.AppendLine(@"            this._context.SaveChanges();");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        public void Remove(Guid id, ViewModels.{0} viewModel)", modelName));
            sb.AppendLine(@"        {");
            sb.AppendLine(string.Format(@"            var model = this._{0}Repository.Get(id);", modelName.ToLower()));
            sb.AppendLine(string.Format(@"            this._{0}Repository.Remove(model);", modelName.ToLower()));
            sb.AppendLine(@"            this._context.SaveChanges();");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        public ViewModels.{0} Get(Guid id)", modelName));
            sb.AppendLine(@"        {");
            sb.AppendLine(string.Format(@"            var model = this._{0}Repository.Get(id);", modelName.ToLower()));
            sb.AppendLine(string.Format(@"            return model != null ? _mapper.Map<ViewModels.{0}>(model) : null;", modelName));
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        public IEnumerable<ViewModels.{0}> GetAll()", modelName));
            sb.AppendLine(@"        {");
            sb.AppendLine(string.Format(@"            var collection = this._{0}Repository.Get();", modelName.ToLower()));
            sb.AppendLine(string.Format(@"            return _mapper.Map<IEnumerable<ViewModels.{0}>>(collection);", modelName));
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateServiceInterfaceText(string modelName, string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine(string.Format(@"using {0}.{1};", namespaceText, VIEWMODELS));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}.{2}", namespaceText, SERVICES, INTERFACES));
            sb.AppendLine(@"{");
            sb.AppendLine(string.Format(@"    public interface I{0}Service", modelName));
            sb.AppendLine(string.Format(@"        : IBaseService<{0}>", modelName));
            sb.AppendLine(@"    {");
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateContextText(string[] names, string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using Microsoft.EntityFrameworkCore;");
            sb.AppendLine(string.Format(@"using {0}.Models;", namespaceText));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}", namespaceText));
            sb.AppendLine(@"{");
            sb.AppendLine(string.Format(@"    public class {0} : DbContext", APPCONTEXT));
            sb.AppendLine(@"    {");
            sb.AppendLine(string.Format(@"        public {0}(DbContextOptions<{0}> options)", APPCONTEXT));
            sb.AppendLine(@"            : base(options)");
            sb.AppendLine(@"        {");
            sb.AppendLine();
            sb.AppendLine(@"        }");
            foreach (var name in names)
            {
                sb.AppendLine();
                sb.AppendLine(string.Format(@"        public DbSet<{0}> {0} {{ get; set; }}", name));
            }
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateBaseRepositoryInterfaceText(string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}.{2}", namespaceText, REPOSITORIES, INTERFACES));
            sb.AppendLine(@"{");
            sb.AppendLine(@"    public interface IBaseRepository<T>");
            sb.AppendLine(@"        where T : class");
            sb.AppendLine(@"    {");
            sb.AppendLine();
            sb.AppendLine(@"        void Add(T model);");
            sb.AppendLine();
            sb.AppendLine(@"        void Add(IEnumerable<T> collection);");
            sb.AppendLine();
            sb.AppendLine(@"        void Update(T model);");
            sb.AppendLine();
            sb.AppendLine(@"        void Remove(T model);");
            sb.AppendLine();
            sb.AppendLine(@"        void Remove(IEnumerable<T> collection);");
            sb.AppendLine();
            sb.AppendLine(@"        T Get(Guid id);");
            sb.AppendLine();
            sb.AppendLine(@"        IEnumerable<T> Get(Func<T, bool> predicate = null);");
            sb.AppendLine();
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateBaseRepositoryText(string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine(@"using System.Linq;");
            sb.AppendLine(string.Format(@"using {0}.Repositories.Interfaces;", namespaceText));
            sb.AppendLine(string.Format(@"using {0}.Common;", namespaceText));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}", namespaceText, REPOSITORIES));
            sb.AppendLine(@"{");
            sb.AppendLine(@"    public abstract class BaseRepository<T> : IBaseRepository<T>");
            sb.AppendLine(@"        where T : GuidEntity");
            sb.AppendLine(@"    {");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        protected {0} _context;", APPCONTEXT));
            sb.AppendLine();
            sb.AppendLine(string.Format(@"        public BaseRepository({0} context)", APPCONTEXT));
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            this._context = context;");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public void Add(IEnumerable<T> collection)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            this._context.Set<T>().AddRange(collection);");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public void Add(T model)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            this._context.Add(model);");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public IEnumerable<T> Get(Func<T, bool> predicate = null)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            IEnumerable<T> collection = this._context.Set<T>();");
            sb.AppendLine(@"            if (predicate != null)");
            sb.AppendLine(@"            {");
            sb.AppendLine(@"                collection = collection.Where(predicate);");
            sb.AppendLine(@"            }");
            sb.AppendLine(@"            return collection;");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public T Get(Guid id)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            return this._context.Set<T>().DefaultIfEmpty(null).SingleOrDefault(e => e.Id == id);");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public void Remove(IEnumerable<T> collection)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            this._context.Set<T>().RemoveRange(collection);");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public void Remove(T model)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            this._context.Set<T>().Remove(model);");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public void Update(T model)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            this._context.Set<T>().Update(model);");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateBaseServiceInterfaceText(string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}.{2}", namespaceText, SERVICES, INTERFACES));
            sb.AppendLine(@"{");
            sb.AppendLine(@"    public interface IBaseService<T>");
            sb.AppendLine(@"        where T : class");
            sb.AppendLine(@"    {");
            sb.AppendLine();
            sb.AppendLine(@"        void Add(T viewModel);");
            sb.AppendLine();
            sb.AppendLine(@"        void Update(Guid id, T viewModel);");
            sb.AppendLine();
            sb.AppendLine(@"        void Remove(Guid id, T viewModel);");
            sb.AppendLine();
            sb.AppendLine(@"        T Get(Guid id);");
            sb.AppendLine();
            sb.AppendLine(@"        IEnumerable<T> GetAll();");
            sb.AppendLine();
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateGuidEntityText(string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using System;");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}.{1}", namespaceText, COMMON));
            sb.AppendLine(@"{");
            sb.AppendLine(@"    public abstract class GuidEntity");
            sb.AppendLine(@"    {");
            sb.AppendLine(@"        int? _requestedHashCode;");
            sb.AppendLine();
            sb.AppendLine(@"        Guid _Id;");
            sb.AppendLine();
            sb.AppendLine(@"        public virtual Guid Id { get; set; }");
            sb.AppendLine();
            sb.AppendLine(@"        public bool IsTransient()");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            return this.Id == Guid.Empty;");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public override bool Equals(object obj)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            if (obj == null || !(obj is GuidEntity))");
            sb.AppendLine(@"                return false;");
            sb.AppendLine(@"            if (Object.ReferenceEquals(this, obj))");
            sb.AppendLine(@"                return true;");
            sb.AppendLine(@"            GuidEntity item = (GuidEntity)obj;");
            sb.AppendLine(@"            if (item.IsTransient() || this.IsTransient())");
            sb.AppendLine(@"                return false;");
            sb.AppendLine(@"            else");
            sb.AppendLine(@"                return item.Id == this.Id;");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public override int GetHashCode()");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            if (!IsTransient())");
            sb.AppendLine(@"            {");
            sb.AppendLine(@"                if (!_requestedHashCode.HasValue)");
            sb.AppendLine(@"                    _requestedHashCode = this.Id.GetHashCode() ^ 31;");
            sb.AppendLine(@"                return _requestedHashCode.Value;");
            sb.AppendLine(@"            }");
            sb.AppendLine(@"            else");
            sb.AppendLine(@"                return base.GetHashCode();");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public static bool operator ==(GuidEntity left, GuidEntity right)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            if (Object.Equals(left, null))");
            sb.AppendLine(@"                return (Object.Equals(right, null)) ? true : false;");
            sb.AppendLine(@"            else");
            sb.AppendLine(@"                return left.Equals(right);");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"        public static bool operator !=(GuidEntity left, GuidEntity right)");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            return !(left == right);");
            sb.AppendLine(@"        }");
            sb.AppendLine();
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }

        public static string GenerateAutoMapperConfigText(string[] names, string namespaceText)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"using AutoMapper;");
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine(@"using System.Linq;");
            sb.AppendLine();
            sb.AppendLine(string.Format(@"namespace {0}", namespaceText));
            sb.AppendLine(@"{");
            sb.AppendLine(@"    public class AutoMapperConfigs : Profile");
            sb.AppendLine(@"    {");
            sb.AppendLine(@"        public AutoMapperConfigs()");
            sb.AppendLine(@"        {");
            sb.AppendLine();
            foreach (var name in names)
            {
                sb.AppendLine(string.Format(@"            #region {0} Model", name));
                sb.AppendLine();
                sb.AppendLine(string.Format(@"            CreateMap<{0}.{2}, {1}.{2}>();", MODELS, VIEWMODELS, name));
                sb.AppendLine();
                sb.AppendLine(string.Format(@"            CreateMap<{0}.{2}, {1}.{2}>();", VIEWMODELS, MODELS, name));
                sb.AppendLine();
                sb.AppendLine(@"            #endregion");
                sb.AppendLine();
            }
            sb.AppendLine(@"        }");
            sb.AppendLine(@"    }");
            sb.AppendLine(@"}");
            return sb.ToString();
        }
    }
}
