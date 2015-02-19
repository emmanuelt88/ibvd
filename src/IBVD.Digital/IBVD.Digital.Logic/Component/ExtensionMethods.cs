using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;

namespace IBVD.Digital.Logic.Component
{
    public static class ExtensionMethods
	{
		public static IQueryable<T> OrderBy<T> (this IQueryable<T> source, string propertyName, bool asc)
		{
			var type = typeof (T);
			string methodName = asc ? "OrderBy" : "OrderByDescending";
			var property = type.GetProperty (propertyName);
			var parameter = Expression.Parameter (type, "p");
			var propertyAccess = Expression.MakeMemberAccess (parameter, property);
			var orderByExp = Expression.Lambda (propertyAccess, parameter);
			MethodCallExpression resultExp = Expression.Call (typeof (Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote (orderByExp));
			return source.Provider.CreateQuery <T> (resultExp);
		}

		public static IQueryable<T> Like<T> (this IQueryable<T> source, string propertyName, string keyword)
		{
			var type = typeof (T);
			var property = type.GetProperty (propertyName);
			var parameter = Expression.Parameter (type, "p");
			var propertyAccess = Expression.MakeMemberAccess (parameter, property);
			var constant = Expression.Constant ("%" + keyword + "%");
			MethodCallExpression methodExp = Expression.Call (null, typeof(SqlMethods).GetMethod ("Like", new Type[] {typeof (string), typeof (string)}), propertyAccess, constant);
			Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>> (methodExp, parameter);
			return source.Where (lambda);
		}
	}
}
