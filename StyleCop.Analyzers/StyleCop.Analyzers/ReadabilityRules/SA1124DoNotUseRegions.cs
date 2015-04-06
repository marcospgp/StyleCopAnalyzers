﻿namespace StyleCop.Analyzers.ReadabilityRules
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// The C# code contains a region.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs whenever a region is placed anywhere within the code. In many editors,
    /// including Visual Studio, the region will appear collapsed by default, hiding the code within the region. It is
    /// generally a bad practice to hide code, as this can lead to bad decisions as the code is maintained over
    /// time.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1124DoNotUseRegions : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1124DoNotUseRegions"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1124";
        private const string Title = "Do not use regions";
        private const string MessageFormat = "Do not use regions";
        private const string Category = "StyleCop.CSharp.ReadabilityRules";
        private const string Description = "The C# code contains a region.";
        private const string HelpLink = "http://www.stylecop.com/docs/SA1124.html";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description, HelpLink);

        private static readonly ImmutableArray<DiagnosticDescriptor> SupportedDiagnosticsValue =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return SupportedDiagnosticsValue;
            }
        }

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(this.HandleRegionDirectiveTrivia, SyntaxKind.RegionDirectiveTrivia);
        }

        private void HandleRegionDirectiveTrivia(SyntaxNodeAnalysisContext context)
        {
            RegionDirectiveTriviaSyntax regionSyntax = context.Node as RegionDirectiveTriviaSyntax;

            // regions that are completely inside a body are handled by SA1123.
            if (regionSyntax != null && !SA1123DoNotPlaceRegionsWithinElements.IsCompletelyContainedInBody(regionSyntax))
            {
                // Regions must not be used.
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, regionSyntax.GetLocation()));
            }
        }
    }
}
