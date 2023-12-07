namespace JobOfferService.Props
{
    public static class RabbitMQJobOffersScraperEventProps
    {
        public const string JOB_OFFERS_SCRAPER_EXCHANGE = "job_offer_scrapper_events";

        public const string OFFERS_CREATE_QUEUE = "offers.create";

        public const string OFFERS_CREATE_ROUTING_KEY = "offers.create";
    }
}
