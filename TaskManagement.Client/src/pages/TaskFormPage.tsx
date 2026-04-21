import React, { useState, useMemo } from 'react';
import { useLoaderData, useNavigate, useParams, Link } from 'react-router';
import { TaskDto, CreateTaskDto, UpdateTaskDto } from '../types';
import { taskService } from '../services/task.service';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { ArrowLeft, Loader2, Save } from 'lucide-react';

export const TaskFormPage = React.memo(() => {
  const task = useLoaderData() as TaskDto | undefined;
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = !!id;

  const [title, setTitle] = useState(task?.title || '');
  const [description, setDescription] = useState(task?.description || '');
  const [isCompleted, setIsCompleted] = useState(task?.isCompleted || false);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');

    if (isEdit) {
      const dto: UpdateTaskDto = { title, description, isCompleted };
      taskService.updateTask(parseInt(id), dto).subscribe({
        next: () => navigate(`/tasks/${id}`),
        error: (err) => {
          setError(err.message || 'Update failed');
          setIsLoading(false);
        }
      });
    } else {
      const dto: CreateTaskDto = { title, description };
      taskService.createTask(dto).subscribe({
        next: (newTask) => navigate(`/tasks/${newTask.id}`),
        error: (err) => {
          setError(err.message || 'Creation failed');
          setIsLoading(false);
        }
      });
    }
  };

  const form = useMemo(() => (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="space-y-2">
        <label className="text-sm font-medium leading-none" htmlFor="title">
          Title
        </label>
        <Input
          id="title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="What needs to be done?"
          required
        />
      </div>
      <div className="space-y-2">
        <label className="text-sm font-medium leading-none" htmlFor="description">
          Description
        </label>
        <textarea
          id="description"
          className="flex min-h-[120px] w-full rounded-md border border-input bg-transparent px-3 py-2 text-sm shadow-sm placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:cursor-not-allowed disabled:opacity-50"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          placeholder="Add more details..."
        />
      </div>
      
      {isEdit && (
        <div className="flex items-center space-x-2 py-2">
          <input
            type="checkbox"
            id="isCompleted"
            className="h-4 w-4 rounded border-gray-300 text-primary focus:ring-primary"
            checked={isCompleted}
            onChange={(e) => setIsCompleted(e.target.checked)}
          />
          <label
            htmlFor="isCompleted"
            className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
          >
            Mark as completed
          </label>
        </div>
      )}

      {error && <p className="text-sm text-destructive font-medium">{error}</p>}

      <div className="flex justify-end gap-3 pt-4 border-t">
        <Button variant="outline" type="button" onClick={() => navigate(-1)} disabled={isLoading}>
          Cancel
        </Button>
        <Button type="submit" disabled={isLoading}>
          {isLoading ? (
            <Loader2 className="mr-2 h-4 w-4 animate-spin" />
          ) : (
            <Save className="mr-2 h-4 w-4" />
          )}
          {isEdit ? 'Save Changes' : 'Create Task'}
        </Button>
      </div>
    </form>
  ), [title, description, isCompleted, isLoading, error, isEdit]);

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <Button variant="ghost" size="sm" asChild className="-ml-2">
        <Link to={isEdit ? `/tasks/${id}` : '/tasks'}>
          <ArrowLeft className="h-4 w-4 mr-2" />
          Back
        </Link>
      </Button>

      <Card>
        <CardHeader>
          <CardTitle className="text-2xl font-bold">
            {isEdit ? 'Edit Task' : 'Create New Task'}
          </CardTitle>
        </CardHeader>
        <CardContent>
          {form}
        </CardContent>
      </Card>
    </div>
  );
});

TaskFormPage.displayName = 'TaskFormPage';
