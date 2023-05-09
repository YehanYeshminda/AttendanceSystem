import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
    TimepickerModule.forRoot(),
    TypeaheadModule.forRoot(),
  ],
  exports: [
    BsDatepickerModule,
    ModalModule,
    ToastrModule,
    TimepickerModule,
    TypeaheadModule,
  ],
})
export class SharedModule {}
