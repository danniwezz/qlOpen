# Running the PricingCalculator

## Prerequisite
To run the application you have to have .NET 7 installed. If you don't have an SQL Server installed on your machine, you need to install that as well.

When an SQL Server is installed, make sure that the connectionstrings are correct. If the SQL Server is installed on localhost then the applications should be good to go.

## Steps:

1. Set all *.api projects as startup projects. 
2. Make sure the connectionstrings in appsettings.json is correct.
3. Run the applications. A web browser should start ands wagger definitions should appear in in the browser.

## Testing the application
1. When running the applications one should first create services in order to get the serviceId.
2. When a service is created then start to define the JSON body for the endpoint `POST customer` in the customer module API. Example:

```
{
  "name": "TestService A",
  "assignedServices": [
    {
      "serviceId": {THE CREATED SERVICE ID},
      "serviceName": "{THE CREATED SERVICES NAME}",
      "price": {THE CREATED SERVICES PRICE},
      "currency": "{THE CREATED SERVICES CURRENCY}",
      "validFromWeekDayNumber": 0,
      "validToWeekDayNumber": 0,
      "startDate": "2023-10-12",
      "endDate": "2023-10-15",
      "discounts": [
        {
          "percentage": 0.68,
          "validFrom": "2023-10-12",
          "validTo": "2023-10-14"
        }
      ]
    }
  ]
}
```
      
Where the `validFromWeekDayNumber` is the number of the weekday that the service starts to charge for the service and `validToWeekDayNumber` is the number of the weekday that the service stops to charge for the service. (Monday = 1 and Sunday = 7)
`percentage` is of type decimal, which means that 1 = 100%

When finished, send the request. A `customerId` should be returned.

3. When a customer is added through the endpoint, it is time to check what the accumulated price for the customer is. 
Open the `PricingCalculator` tab/window in your browser and insert the `customerId` and a date `calculateUntilDate` that is the date that you want to calculate the costs unti (If a date is not provided today's date will be used).

The response of the request should show the accumulated price for the customer.


## Running the test cases
 Test cases 1 and 2 are located under PricingCalculator.Tests and are named the same to avoid confusion.
