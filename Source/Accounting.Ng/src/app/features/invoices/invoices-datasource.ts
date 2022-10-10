import { DataSource } from '@angular/cdk/collections';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { map } from 'rxjs/operators';
import { Observable, of as observableOf, merge } from 'rxjs';

// TODO: Replace this with your own data model type
export interface InvoicesItem {
  ref: string;
  amount: string;
  name: string;
}

// TODO: replace this with real data from your application
const EXAMPLE_DATA: InvoicesItem[] = [
  {ref: 'NG-1', amount: '10.00', name: 'Labour'},
  {ref: 'NG-2', amount: '20.00', name: 'Parts'},
  {ref: 'NG-3', amount: '110.00', name: 'Parts'},
  {ref: 'NG-4', amount: '230.00', name: 'Service'},
  {ref: 'NG-5', amount: '50.00', name: 'Labour'},
  {ref: 'NG-6', amount: '70.00', name: 'Service'},
  {ref: 'NG-7', amount: '33.00', name: 'Labour'},
  {ref: 'NG-8', amount: '370.00', name: 'Parts'},
  {ref: 'NG-9', amount: '1100.00', name: 'Parts'},
  {ref: 'NG-10', amount: '100.00', name: 'Service'},
  {ref: 'NG-11', amount: '70.00', name: 'Labour'},
  {ref: 'NG-12', amount: '78.00', name: 'Labour'},
  {ref: 'NG-13', amount: '490.00', name: 'Service'},
  {ref: 'NG-14', amount: '430.00', name: 'Labour'},
  {ref: 'NG-15', amount: '3.99', name: 'Parts'},
  {ref: 'NG-16', amount: '33.00', name: 'Parts'},
  {ref: 'NG-17', amount: '63.00', name: 'Service'},
  {ref: 'NG-18', amount: '57.00', name: 'Labour'},
  {ref: 'NG-19', amount: '56.00', name: 'Parts'},
  {ref: 'NG-20', amount: '23.00', name: 'Parts'},
  {ref: 'NG-21', amount: '23.00', name: 'Service'},
  {ref: 'NG-22', amount: '0.99', name: 'Labour'},
  {ref: 'NG-23', amount: '15.99', name: 'Service'},
];

/**
 * Data source for the Invoices view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
export class InvoicesDataSource extends DataSource<InvoicesItem> {
  data: InvoicesItem[] = EXAMPLE_DATA;
  paginator: MatPaginator | undefined;
  sort: MatSort | undefined;

  constructor() {
    super();
  }

  /**
   * Connect this data source to the table. The table will only update when
   * the returned stream emits new items.
   * @returns A stream of the items to be rendered.
   */
  connect(): Observable<InvoicesItem[]> {
    if (this.paginator && this.sort) {
      // Combine everything that affects the rendered data into one update
      // stream for the data-table to consume.
      return merge(observableOf(this.data), this.paginator.page, this.sort.sortChange)
        .pipe(map(() => {
          return this.getPagedData(this.getSortedData([...this.data ]));
        }));
    } else {
      throw Error('Please set the paginator and sort on the data source before connecting.');
    }
  }

  /**
   *  Called when the table is being destroyed. Use this function, to clean up
   * any open connections or free any held resources that were set up during connect.
   */
  disconnect(): void {}

  /**
   * Paginate the data (client-side). If you're using server-side pagination,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getPagedData(data: InvoicesItem[]): InvoicesItem[] {
    if (this.paginator) {
      const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
      return data.splice(startIndex, this.paginator.pageSize);
    } else {
      return data;
    }
  }

  /**
   * Sort the data (client-side). If you're using server-side sorting,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getSortedData(data: InvoicesItem[]): InvoicesItem[] {
    if (!this.sort || !this.sort.active || this.sort.direction === '') {
      return data;
    }

    return data.sort((a, b) => {
      const isAsc = this.sort?.direction === 'asc';
      switch (this.sort?.active) {
        case 'name': return compare(a.name, b.name, isAsc);
        case 'id': return compare(+a.ref, +b.ref, isAsc);
        default: return 0;
      }
    });
  }
}

/** Simple sort comparator for example ref/name columns (for client-side sorting). */
function compare(a: string | number, b: string | number, isAsc: boolean): number {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
