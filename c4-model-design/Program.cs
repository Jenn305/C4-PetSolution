using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderModels();
        }

        static void RenderModels()
        {
            const long workspaceId = 77366;
            const string apiKey = "17c1766c-d97d-4b1f-a777-941023293b15";
            const string apiSecret = "2113c618-1eb6-4b31-ad26-7aef4d5d5cf8";
            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret); 

            Workspace workspace = new Workspace("C4 Model- Sistema de intermediarios PetCare", "Sistema de intermediarios en el servicio de mascotas, PetCare");

            ViewSet viewSet = workspace.Views;

            Model model = workspace.Model; //*

            // 1. DIAGRAMA DE CONTEXTO
            SoftwareSystem monitoringSystem = model.AddSoftwareSystem("Sistema de intermediarios en el servicio de mascotas, PetCare", "Permite dar a conocer servicios que brinden cuidado a las mascotas");
            SoftwareSystem googleMaps = model.AddSoftwareSystem("Google Maps", "Plataforma que ofrece una REST API de información geo referencial de todos nuestros usuarios.");

            Person dueno_mascota = model.AddPerson("Dueño de mascota", "Ciudadano peruano.");
            Person miembro_teamcare = model.AddPerson("Miembro de TeamCare", "Ciudadano peruano.");
            Person persona_que_da_adopcion = model.AddPerson("Personas que dan en adopcion a mascotas", "Ciudadano peruano.");
            Person amigo_invitado = model.AddPerson("Amigo Invitado", "Ciudadano peruano.");
            Person tienda = model.AddPerson("Tiendas", "Negocios que ofrecen servicio y/o productos para mascotas.");
            Person centro_adopcion = model.AddPerson("Centro de Adopcion", "Centros de adopcion disponibles y seguros.");
            Person admin = model.AddPerson("Admin", "User Admin.");

            dueno_mascota.Uses(monitoringSystem, "Realiza consultas para conocer tiendas y/o personas que brinden servicios parasu mascota");
            miembro_teamcare.Uses(monitoringSystem, "Realiza un perfil para ofrecer un servicio sencillo para con las mascotas");
            persona_que_da_adopcion.Uses(monitoringSystem, "Da a conocer crias de sus mascotas para que otros puedan adoptarlas");
            amigo_invitado.Uses(monitoringSystem, "Se crea un perfil para poder acceder a todas lasfuncionalidades  de la app");
            tienda.Uses(monitoringSystem, "Da a concer su negocio para su posterior visita en su pagina oficial");
            centro_adopcion.Uses(monitoringSystem, "Da a conocer el centro para su posterior visita en su pagina oficial");

            admin.Uses(monitoringSystem, "Realiza consultas para mantenerse al tanto de la interaccion entre los usuarios que ofrecen y los que buscan servicios y/o negocios");


            monitoringSystem.Uses(googleMaps, "Usa la API de google maps");

            // Tags
            dueno_mascota.AddTags("Ciudadano");
            miembro_teamcare.AddTags("Ciudadano");
            persona_que_da_adopcion.AddTags("Ciudadano");
            amigo_invitado.AddTags("Ciudadano");
            tienda.AddTags("Ciudadano");
            centro_adopcion.AddTags("Ciudadano");
            admin.AddTags("Admin");

            monitoringSystem.AddTags("SistemaMonitoreo");
            googleMaps.AddTags("GoogleMaps");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Ciudadano") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Admin") { Background = "#aa60af", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("SistemaMonitoreo") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("GoogleMaps") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });

            SystemContextView contextView = viewSet.CreateSystemContextView(monitoringSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            //*

            // 2. Diagrama de Contenedores
            Container mobileApplication = monitoringSystem.AddContainer("Mobile App", "Permite a los usuarios visualizar un dashboard con el resumen de toda la información del traslado de los lotes de vacunas.", "Swift UI");
            Container webApplication = monitoringSystem.AddContainer("Web App", "Permite a los usuarios visualizar un dashboard con el resumen de toda la información del traslado de los lotes de vacunas.", "React");
            Container landingPage = monitoringSystem.AddContainer("Landing Page", "", "React");
            Container apiRest = monitoringSystem.AddContainer("API REST", "API Rest", "NodeJS (NestJS) port 8080");

            Container flightPlanningContext = monitoringSystem.AddContainer("Flight Planning Context", "Bounded Context de Planificación de Vuelos", "NodeJS (NestJS)");
            Container airportContext = monitoringSystem.AddContainer("Airport Context", "Bounded Context de información de Aeropuertos", "NodeJS (NestJS)");
            Container aircraftInventoryContext = monitoringSystem.AddContainer("Aircraft Inventory Context", "Bounded Context de Inventario de Aviones", "NodeJS (NestJS)");
            Container vaccinesInventoryContext = monitoringSystem.AddContainer("Vaccines Inventory Context", "Bounded Context de Inventario de Vacunas", "NodeJS (NestJS)");
            Container monitoringContext = monitoringSystem.AddContainer("Monitoring Context", "Bounded Context de Monitoreo en tiempo real del status y ubicación del vuelo que transporta las vacunas", "NodeJS (NestJS)");
            Container securityContext = monitoringSystem.AddContainer("Security Context", "Bounded Context de Seguridad", "NodeJS (NestJS)");

            Container database = monitoringSystem.AddContainer("Database", "", "Oracle");

            dueno_mascota.Uses(mobileApplication, "Consulta");
            dueno_mascota.Uses(webApplication, "Consulta");
            dueno_mascota.Uses(landingPage, "Consulta");

            admin.Uses(mobileApplication, "Consulta");
            admin.Uses(webApplication, "Consulta");
            admin.Uses(landingPage, "Consulta");

            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");

            apiRest.Uses(flightPlanningContext, "", "");
            apiRest.Uses(airportContext, "", "");
            apiRest.Uses(aircraftInventoryContext, "", "");
            apiRest.Uses(vaccinesInventoryContext, "", "");
            apiRest.Uses(monitoringContext, "", "");
            apiRest.Uses(securityContext, "", "");

            flightPlanningContext.Uses(database, "", "");
            airportContext.Uses(database, "", "");
            aircraftInventoryContext.Uses(database, "", "");
            vaccinesInventoryContext.Uses(database, "", "");
            monitoringContext.Uses(database, "", "");
            securityContext.Uses(database, "", "");

            monitoringContext.Uses(googleMaps, "API Request", "JSON/HTTPS");
            //monitoringContext.Uses(aircraftSystem, "API Request", "JSON/HTTPS");

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            database.AddTags("Database");

            string contextTag = "Context";

            flightPlanningContext.AddTags(contextTag);
            airportContext.AddTags(contextTag);
            aircraftInventoryContext.AddTags(contextTag);
            vaccinesInventoryContext.AddTags(contextTag);
            monitoringContext.AddTags(contextTag);
            securityContext.AddTags(contextTag);

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIRest") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle(contextTag) { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(monitoringSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3. Diagrama de Componentes (Monitoring Context)
            Component domainLayer = monitoringContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");

            Component monitoringController = monitoringContext.AddComponent("MonitoringController", "REST API endpoints de monitoreo.", "NodeJS (NestJS) REST Controller");

            Component monitoringApplicationService = monitoringContext.AddComponent("MonitoringApplicationService", "Provee métodos para el monitoreo, pertenece a la capa Application de DDD", "NestJS Component");

            Component flightRepository = monitoringContext.AddComponent("FlightRepository", "Información del vuelo", "NestJS Component");
            Component vaccineLoteRepository = monitoringContext.AddComponent("VaccineLoteRepository", "Información de lote de vacunas", "NestJS Component");
            Component locationRepository = monitoringContext.AddComponent("LocationRepository", "Ubicación del vuelo", "NestJS Component");

            Component aircraftSystemFacade = monitoringContext.AddComponent("Aircraft System Facade", "", "NestJS Component");

            apiRest.Uses(monitoringController, "", "JSON/HTTPS");
            monitoringController.Uses(monitoringApplicationService, "Invoca métodos de monitoreo");

            monitoringApplicationService.Uses(domainLayer, "Usa", "");
            monitoringApplicationService.Uses(aircraftSystemFacade, "Usa");
            monitoringApplicationService.Uses(flightRepository, "", "");
            monitoringApplicationService.Uses(vaccineLoteRepository, "", "");
            monitoringApplicationService.Uses(locationRepository, "", "");

            flightRepository.Uses(database, "", "");
            vaccineLoteRepository.Uses(database, "", "");
            locationRepository.Uses(database, "", "");

            locationRepository.Uses(googleMaps, "", "JSON/HTTPS");

            //aircraftSystemFacade.Uses(aircraftSystem, "JSON/HTTPS");

            // Tags
            domainLayer.AddTags("DomainLayer");
            monitoringController.AddTags("MonitoringController");
            monitoringApplicationService.AddTags("MonitoringApplicationService");
            flightRepository.AddTags("FlightRepository");
            vaccineLoteRepository.AddTags("VaccineLoteRepository");
            locationRepository.AddTags("LocationRepository");
            aircraftSystemFacade.AddTags("AircraftSystemFacade");

            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("VaccineLoteRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AircraftSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(monitoringContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(mobileApplication);
            componentView.Add(webApplication);
            componentView.Add(apiRest);
            componentView.Add(database);
            //componentView.Add(aircraftSystem);
            componentView.Add(googleMaps);
            componentView.AddAllComponents();


            //4.Component 2
             monitoringContext.AddComponent("Domain Layer2", "", "NodeJS (NestJS)");

             monitoringContext.AddComponent("MonitoringController2", "REST API endpoyints de monitoreo.", "NodeJS (NestJS) REST Controller");

             monitoringContext.AddComponent("MonitoringApplicationService2", "Provee métodos para el monitoreo, pertenece a la capa Application de DDD", "NestJS Component");

             monitoringContext.AddComponent("FlightRepository2", "Información del vuelo", "NestJS Component");
             monitoringContext.AddComponent("VaccineLoteRepository2", "Información de lote de vacunas", "NestJS Component");
             monitoringContext.AddComponent("LocationRepository2", "Ubicación del vuelo", "NestJS Component");

             monitoringContext.AddComponent("Aircraft System Facade2", "", "NestJS Component");

            apiRest.Uses(monitoringController, "", "JSON/HTTPS");
            monitoringController.Uses(monitoringApplicationService, "Invoca métodos de monitoreo");

            monitoringApplicationService.Uses(domainLayer, "Usa", "");
            monitoringApplicationService.Uses(aircraftSystemFacade, "Usa");
            monitoringApplicationService.Uses(flightRepository, "", "");
            monitoringApplicationService.Uses(vaccineLoteRepository, "", "");
            monitoringApplicationService.Uses(locationRepository, "", "");

            flightRepository.Uses(database, "", "");
            vaccineLoteRepository.Uses(database, "", "");
            locationRepository.Uses(database, "", "");

            locationRepository.Uses(googleMaps, "", "JSON/HTTPS");

            //aircraftSystemFacade.Uses(aircraftSystem, "JSON/HTTPS");

            // Tags
            domainLayer.AddTags("DomainLayer");
            monitoringController.AddTags("MonitoringController");
            monitoringApplicationService.AddTags("MonitoringApplicationService");
            flightRepository.AddTags("FlightRepository");
            vaccineLoteRepository.AddTags("VaccineLoteRepository");
            locationRepository.AddTags("LocationRepository");
            aircraftSystemFacade.AddTags("AircraftSystemFacade");

            styles.Add(new ElementStyle("DomainLayer2") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("VaccineLoteRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AircraftSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

             viewSet.CreateComponentView(monitoringContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(mobileApplication);
            componentView.Add(webApplication);
            componentView.Add(apiRest);
            componentView.Add(database);
            //componentView.Add(aircraftSystem);
            componentView.Add(googleMaps);
            componentView.AddAllComponents();





            //obligatorio
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}