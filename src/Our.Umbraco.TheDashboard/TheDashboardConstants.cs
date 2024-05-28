using Umbraco.Cms.Core.Models;

namespace Our.Umbraco.TheDashboard
{
	public static class TheDashboardConstants
	{
		public static class ActivityTypes
		{
			public const string Publish = "Publish";
			public const string Save = "Save";
			public const string SaveAndScheduled = "SaveAndScheduled";
			public const string RollBack = "RollBack";
			public const string RecycleBin = "RecycleBin";
			public const string Unpublish = "Unpublish";
		}

		public static class UmbracoLogHeaders
		{
			public const string Publish = nameof(AuditType.Publish);
			public const string PublishVariant = nameof(AuditType.PublishVariant);
			public const string Save = nameof(AuditType.Save);
			public const string SaveVariant = nameof(AuditType.SaveVariant);
			public const string RollBack = nameof(AuditType.RollBack);
			public const string Move = nameof(AuditType.Move);
			public const string Delete = nameof(AuditType.Delete);
			public const string Unpublish = nameof(AuditType.Unpublish);
			public const string UnpublishVariant = nameof(AuditType.UnpublishVariant);
			public const string Copy = nameof(AuditType.Copy);
			public const string Sort = nameof(AuditType.Sort);
		}
	}
}
