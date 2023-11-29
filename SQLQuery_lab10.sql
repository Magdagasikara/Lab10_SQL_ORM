-- Lab 10
-- Magdalena Kubien, NET23

use Northwind
go

-- H�mta alla produkter med deras namn, pris och kategori namn. Sortera p� kategori namn och sen produkt namn
select 
	c.CategoryName,
	p.ProductName,
	p.UnitPrice
from Products p 
	left join Categories c on p.CategoryID=c.CategoryID
order by 
	c.CategoryName,
	p.ProductName;

-- H�mta alla kunder och antal ordrar de gjort. Sortera fallande p� antal ordrar. ++ alternativ f�r antal ordrar: summan f�r deras totala orderv�rde
select
	c.CompanyName,
	count(o.OrderId) as OrdersCount,
	case
		when count(o.OrderId)=0 then 0
		else sum(od.UnitPrice*od.Quantity) 
	end as OrdersTotalValue
from Customers c 
	left join Orders o on c.CustomerID=o.CustomerID
	left join [Order Details] od on od.OrderID=o.OrderID
group by 
	c.CompanyName
order by 
	OrdersCount desc

-- H�mta alla anst�llda tillsammans med territorie de har hand om (EmployeeTerritories och Territories tabellerna)
select 
	e.FirstName,
	e.LastName,
	t.TerritoryDescription
from Employees e
	left join EmployeeTerritories et on e.EmployeeID=et.EmployeeID
	left join Territories t on et.TerritoryID=t.TerritoryID
	-- sometimes more than one territory per employee it seems but I leave it for now