FROM mcr.microsoft.com/dotnet/sdk:3.1 AS test

WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY Grand.Domain/*.csproj ./Grand.Domain/
COPY Grand.Core/*.csproj ./Grand.Core/
COPY Grand.Framework/*.csproj ./Grand.Framework/
COPY Grand.Services/*.csproj ./Grand.Services/
COPY Grand.Api/*.csproj ./Grand.Api/
COPY Grand.Web/*.csproj ./Grand.Web/

# Copy test project files
COPY Tests/Grand.Core.Tests/*.csproj ./Tests/Grand.Core.Tests/
COPY Tests/Grand.Services.Tests/*.csproj ./Tests/Grand.Services.Tests/
COPY Tests/Grand.Api.Tests/*.csproj ./Tests/Grand.Api.Tests/
COPY Tests/Grand.Plugin.Tests/*.csproj ./Tests/Grand.Plugin.Tests/

# Copy plugin project files
COPY Plugins/Grand.Plugin.DiscountRequirements.Standard/*.csproj ./Plugins/Grand.Plugin.DiscountRequirements.Standard/
COPY Plugins/Grand.Plugin.ExchangeRate.McExchange/*.csproj ./Plugins/Grand.Plugin.ExchangeRate.McExchange/
COPY Plugins/Grand.Plugin.ExternalAuth.Facebook/*.csproj ./Plugins/Grand.Plugin.ExternalAuth.Facebook/
COPY Plugins/Grand.Plugin.ExternalAuth.Google/*.csproj ./Plugins/Grand.Plugin.ExternalAuth.Google/
COPY Plugins/Grand.Plugin.Payments.BrainTree/*.csproj ./Plugins/Grand.Plugin.Payments.BrainTree/
COPY Plugins/Grand.Plugin.Payments.CashOnDelivery/*.csproj ./Plugins/Grand.Plugin.Payments.CashOnDelivery/
COPY Plugins/Grand.Plugin.Payments.PayPalStandard/*.csproj ./Plugins/Grand.Plugin.Payments.PayPalStandard/
COPY Plugins/Grand.Plugin.Shipping.ByWeight/*.csproj ./Plugins/Grand.Plugin.Shipping.ByWeight/
COPY Plugins/Grand.Plugin.Shipping.FixedRateShipping/*.csproj ./Plugins/Grand.Plugin.Shipping.FixedRateShipping/
COPY Plugins/Grand.Plugin.Shipping.ShippingPoint/*.csproj ./Plugins/Grand.Plugin.Shipping.ShippingPoint/
COPY Plugins/Grand.Plugin.Tax.CountryStateZip/*.csproj ./Plugins/Grand.Plugin.Tax.CountryStateZip/
COPY Plugins/Grand.Plugin.Tax.FixedRate/*.csproj ./Plugins/Grand.Plugin.Tax.FixedRate/
COPY Plugins/Grand.Plugin.Widgets.GoogleAnalytics/*.csproj ./Plugins/Grand.Plugin.Widgets.GoogleAnalytics/
COPY Plugins/Grand.Plugin.Widgets.FacebookPixel/*.csproj ./Plugins/Grand.Plugin.Widgets.FacebookPixel/
COPY Plugins/Grand.Plugin.Widgets.Slider/*.csproj ./Plugins/Grand.Plugin.Widgets.Slider/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Install ReportGenerator tool
RUN dotnet tool install -g dotnet-reportgenerator-globaltool

# Add dotnet tools to PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Run tests with coverage
#CMD ["dotnet", "test"] 
CMD ["bash", "-c", "echo GLIBC VERSION && ldd --version && echo GLIBC VERSION CHECK && dotnet test"]
