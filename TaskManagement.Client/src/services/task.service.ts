import { BehaviorSubject, Observable, from, map, shareReplay, switchMap, tap, of } from 'rxjs';
import { TaskSummaryDto, TaskDto, CreateTaskDto, UpdateTaskDto } from '../types';
import { authService } from './auth.service';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

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

  public fetchTasks(): Observable<TaskSummaryDto[]> {
    return from(fetch(`${API_URL}/tasks`, {
      headers: this.getHeaders()
    }).then(res => {
      if (!res.ok) throw new Error('Failed to fetch tasks');
      return res.json();
    })).pipe(
      tap(tasks => this.tasksSubject.next(tasks)),
      shareReplay(1)
    );
  }

  public getTask(id: number): Observable<TaskDto> {
    if (!this.taskCache.has(id)) {
      this.taskCache.set(id, new BehaviorSubject<TaskDto | null>(null));
    }

    return from(fetch(`${API_URL}/tasks/${id}`, {
      headers: this.getHeaders()
    }).then(res => {
      if (!res.ok) throw new Error('Failed to fetch task');
      return res.json();
    })).pipe(
      tap(task => this.taskCache.get(id)!.next(task)),
      shareReplay(1)
    );
  }

  public createTask(dto: CreateTaskDto): Observable<TaskDto> {
    return from(fetch(`${API_URL}/tasks`, {
      method: 'POST',
      headers: this.getHeaders(),
      body: JSON.stringify(dto)
    }).then(res => {
      if (!res.ok) throw new Error('Failed to create task');
      return res.json();
    })).pipe(
      tap(() => this.fetchTasks().subscribe()), // Refresh list
      shareReplay(1)
    );
  }

  public updateTask(id: number, dto: UpdateTaskDto): Observable<TaskDto> {
    return from(fetch(`${API_URL}/tasks/${id}`, {
      method: 'PUT',
      headers: this.getHeaders(),
      body: JSON.stringify(dto)
    }).then(res => {
      if (!res.ok) throw new Error('Failed to update task');
      return res.json();
    })).pipe(
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
    return from(fetch(`${API_URL}/tasks/${id}`, {
      method: 'DELETE',
      headers: this.getHeaders()
    }).then(res => {
      if (!res.ok) throw new Error('Failed to delete task');
    })).pipe(
      tap(() => {
        this.taskCache.delete(id);
        this.fetchTasks().subscribe(); // Refresh list
      }),
      shareReplay(1)
    );
  }
}

export const taskService = new TaskService();
