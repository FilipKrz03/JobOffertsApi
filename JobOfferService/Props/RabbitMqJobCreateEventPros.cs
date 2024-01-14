namespace JobOffersService.Props
{
    public static class RabbitMqJobCreateEventPros
    {
        public const string JOB_OFFER_EXCHANGE = "job_events";

        public const string JOB_CREATE_QUEUE = "job_offer.create";
        public const string JOB_CREATE_ROUTING_KEY = "job_offer.create";
        public const string JOB_CREATE_CLIENT_PROVIDED_NAME = "Job Create Event Reciver";
    }
}
