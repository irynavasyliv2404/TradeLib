### Add a parameter specifying the amount of pumpkins to buy or sell 

1) add property "Amount" to Order model.
2) find the price of one pumpkin that buyer/seller wants to buy/sell.
3) find order where amount and price are matched (seller's amount is not less than buyer's and buyer's price per pumpkin is not less than seller's)
4) if the matching order was found - make the trade, if there are multiple suitable buy/sell orders, choose with the hightest/lowest price per one pumpkin.
5) the matching order cannot take part in trades anymore, because "An order can be matched only once".


### Add possibility to specify expiration time for the order.

1) add condition when matching the order, that it is not expired. 
2) Cleanup the stored orders. There are two ways to do it:
* every time when new order comes, look for expired orders and delete them.
* use cron to set the removing order method be called in time the order is expired.
