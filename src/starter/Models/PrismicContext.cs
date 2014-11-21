﻿using System;
using System.Linq;
using System.Collections.Generic;
using prismic;
using prismic.extensions;

namespace prismic.mvc.starter
{
	public class PrismicContext
	{
		readonly prismic.Api api;
		readonly string maybeRef;
		readonly string maybeAccessToken;
		readonly prismic.DocumentLinkResolver linkResolver;

		public PrismicContext(){}

		public PrismicContext(prismic.Api api, string maybeRef, string maybeAccessToken, prismic.DocumentLinkResolver linkResolver)
		{
			this.api = api;
			this.maybeRef = maybeRef;
			this.maybeAccessToken = maybeAccessToken;
			this.linkResolver = linkResolver;
		}
		public prismic.Api Api { get { return this.api; } }
		public string MaybeRef { get { return this.maybeRef; } }

		public prismic.DocumentLinkResolver LinkResolver
		{
			get { return this.linkResolver; }
		}

		public string ResolveLink(prismic.Document document)
		{
			return this.linkResolver.Resolve (document);
		}


		public IEnumerable<prismic.Ref> FutureReleasesRefs
		{
			get {
				Func<DateTime?, Int64> mapScheduledAt = scheduledAt => scheduledAt.HasValue ? scheduledAt.Value.Ticks : 0;
				return this.api.Refs
					.Where (@ref => !@ref.IsMasterRef)
					.OrderBy (@ref => mapScheduledAt (@ref.ScheduledAt))
					.ToList ();
			}
		}
	}
}

