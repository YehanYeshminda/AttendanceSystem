export interface CreateEmployee {
  regNo: string;
  enrollNo: string;
  username: string;
  designation: string;
  dateOfJoin: Date;
  dateOfBirth: Date;
  status: number;
  file: File;
}

export interface Employee {
  id: number;
  regNo: string;
  enrollNo: string;
  username: string;
  designation: string;
  dateOfJoin: string;
  dateOfBirth: string;
  status: number;
  pictureUrl: string;
}
