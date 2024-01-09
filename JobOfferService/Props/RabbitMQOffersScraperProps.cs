namespace JobOfferService.Props
{
    public static class RabbitMQOffersScraperProps
    {
        public const string OFFERS_SCRAPPER_CLIENT_PROVIDED_NAME = "Offers Scrapper Events Producer";

        public const string OFFERS_SCRAPER_EXCHANGE = "offers_scrapper_events";

        public const string OFFERS_CREATE_QUEUE = "offers.create";
        public const string OFFERS_CREATE_ROUTING_KEY = "offers.create";
        public const string OFFERS_CREATE_MESSAGE = "offers.create";

        public const string OFFERS_UPDATE_QUEUE = "offers.update";
        public const string OFFERS_UPDATE_ROUTING_KEY = "offers.update";
        public const string OFFERS_UPDATE_MESSAGE = "offers.update";
    }
}
