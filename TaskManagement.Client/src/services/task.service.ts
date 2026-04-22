import { BehaviorSubject, Observable, from, shareReplay, tap } from 'rxjs';
import { TaskSummaryDto, TaskDto, CreateTaskDto, UpdateTaskDto } from '../types';
import { authService } from './auth.service';
import { configService } from './config.service';
import { ApiError } from './api.error';
import { apiClient } from './api.client';

class TaskService {
  private tasksSubject = new BehaviorSubject<TaskSummaryDto[]>([]);
  public tasks$ = this.tasksSubject.asObservable().pipe(shareReplay(1));

  private taskCache = new Map<number, BehaviorSubject<TaskDto | null>>();

  private getHeaders() {
    return {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${authService.token}`
    };
  }

  private async handleResponse(res: Response, errorMessage: string) {
    if (!res.ok) {
      throw new ApiError(res.status, errorMessage);
    }
    // DELETE requests might not have a body
    if (res.status === 204 || res.headers.get('content-length') === '0') {
      return null;
    }
    return res.json();
  }

  public fetchTasks(): Observable<TaskSummaryDto[]> {
    return from(apiClient(`${configService.apiUrl}/tasks`, {
      headers: this.getHeaders()
    }).then(res => this.handleResponse(res, 'Failed to fetch tasks'))).pipe(
      tap(tasks => this.tasksSubject.next(tasks)),
      shareReplay(1)
    );
  }

  public getTask(id: number): Observable<TaskDto> {
    if (!this.taskCache.has(id)) {
      this.taskCache.set(id, new BehaviorSubject<TaskDto | null>(null));
    }

    return from(apiClient(`${configService.apiUrl}/tasks/${id}`, {
      headers: this.getHeaders()
    }).then(res => this.handleResponse(res, 'Failed to fetch task'))).pipe(
      tap(task => this.taskCache.get(id)!.next(task)),
      shareReplay(1)
    );
  }

  public createTask(dto: CreateTaskDto): Observable<TaskDto> {
    return from(apiClient(`${configService.apiUrl}/tasks`, {
      method: 'POST',
      headers: this.getHeaders(),
      body: JSON.stringify(dto)
    }).then(res => this.handleResponse(res, 'Failed to create task'))).pipe(
      tap(() => this.fetchTasks().subscribe()), // Refresh list
      shareReplay(1)
    );
  }

  public updateTask(id: number, dto: UpdateTaskDto): Observable<TaskDto> {
    return from(apiClient(`${configService.apiUrl}/tasks/${id}`, {
      method: 'PUT',
      headers: this.getHeaders(),
      body: JSON.stringify(dto)
    }).then(res => this.handleResponse(res, 'Failed to update task'))).pipe(
      tap(updatedTask => {
        if (this.taskCache.has(id)) {
          this.taskCache.get(id)!.next(updatedTask);
        }
        this.fetchTasks().subscribe(); // Refresh list
      }),
      shareReplay(1)
    );
  }

  public deleteTask(id: number): Observable<void> {
    return from(apiClient(`${configService.apiUrl}/tasks/${id}`, {
      method: 'DELETE',
      headers: this.getHeaders()
    }).then(res => this.handleResponse(res, 'Failed to delete task'))).pipe(
      tap(() => {
        this.taskCache.delete(id);
        this.fetchTasks().subscribe(); // Refresh list
      }),
      shareReplay(1)
    );
  }
}

export const taskService = new TaskService();
