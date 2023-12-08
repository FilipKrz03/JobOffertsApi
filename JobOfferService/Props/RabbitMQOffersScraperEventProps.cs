namespace JobOfferService.Props
{
    public static class RabbitMQOffersScraperEventProps
    {

        public const string OFFERS_SCRAPPER_CLIENT_PROVIDED_NAME = "Offers Scrapper Events Producer";
        public const string OFFERS_SCRAPER_EXCHANGE = "offers_scrapper_events";

        public const string OFFERS_CREATE_QUEUE = "offers.create";

        public const string OFFERS_CREATE_ROUTING_KEY = "offers.create";
    }
}
